using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Exceptions;
using Microsoft.EntityFrameworkCore;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public class StoreService : IStoreService
    {
        private readonly CocomeDbContext _context;
        private readonly IModelMapper _mapper;

        public StoreService(
            CocomeDbContext context,
            IModelMapper mapper
        )
        {
            _mapper = mapper;
            _context = context;
        }

        public Store GetStore(int storeId)
        {
            Store store = _context.Stores.Find(storeId);
            if (store == null)
            {
                throw new EntityNotFoundException("store with id " + storeId + " could not be found");
            }
            return store;
        }

        public IEnumerable<Store> GetAllStores()
        {
            return _context.Stores;
        }

        public IEnumerable<OrderTO> GetOrders(int storeId)
        {
            return _context.Orders
                .Where(order => order.StoreId == storeId)
                .Include(order => order.Store)
                .Include(order => order.Provider)
                .AsEnumerable()
                .GroupJoin(_context.OrderElements
                    .Include(element => element.Product),
                    order => order.Id,
                    element => element.OrderId,
                    (order, elements) => _mapper.CreateOrderTO(order, elements.AsEnumerable()));
        }


        public void CloseOrder(int storeId, int orderId)
        {
            Order order = _context.Orders.Find(orderId);

            if (order == null)
            {
                throw new EntityNotFoundException("order with id " + orderId + " could not be found");
            }

            if (order.StoreId != storeId)
            {
                throw new CrossAccessException("no access to order " + orderId);
            }

            var orderElements = _context.OrderElements
                .Where(element => element.OrderId == orderId)
                .Include(element => element.Product);

            foreach(var element in orderElements)
            {
                StockItem item = _context.StockItems
                    .Where(item => item.ProductId == element.ProductId && item.StoreId == storeId)
                    .SingleOrDefault();
                item.Stock += element.Amount;
            };

            order.Closed = true;
            _context.SaveChanges();
        }

        public void PlaceOrder(int storeId, IEnumerable<OrderElementTO> elements)
        {

            DateTime dateTime = DateTime.Now;
            Store store = GetStore(storeId);
            if (store == null)
            {
                throw new EntityNotFoundException("store with id " + storeId + " could not be found");
            }

            var groupedElements = elements.GroupBy(element => element.Product.Provider.Id);
            foreach (var element in groupedElements)
            {
                Order order = new()
                {
                    ProviderId = element.Key,
                    StoreId = storeId,
                    PlacingDate = dateTime,
                    DeliveringDate = dateTime,
                    Delivered = false,
                    Closed = false,
                };
                
                var orderElements = element.ToArray()
                    .Select(element => _mapper.CreateOrderElement(order, element))
                    .ToArray();

                _context.AddRange(orderElements);
            };
            _context.SaveChanges();
        }


        public IEnumerable<StockItem> GetInventory(int storeId)
        {
            return _context.StockItems
                .Where(item => item.Store.Id == storeId)
                .Include(item => item.Store)
                .Include(item => item.Product)
                .ThenInclude(product => product.Provider);
        }


        public ProductTO GetProduct(int storeId, int productId)
        {
            Product product = _context.StockItems
                .Where(item => item.StoreId == storeId && item.ProductId == productId)
                .Include(item => item.Product)
                .ThenInclude(product => product.Provider)
                .Select(item => item.Product)
                .SingleOrDefault();

            if (product == null)
            {
                throw new CrossAccessException(
                    "product " + productId + " is not accessable in store " + storeId);
            }

            return _mapper.CreateProductTO(product);
        }

        public void UpdateProduct(int storeId, ProductTO productTO)
        {
            Product product = _context.Products.Find(productTO.Id);
            _mapper.UpdateProduct(product, productTO);
            _context.SaveChanges();
        }

        public void UpdateStock(int storeId, int productId, int stock)
        {
            StockItem item = _context.StockItems
                .Where(item => item.Store.Id == storeId && item.Product.Id == productId)
                .SingleOrDefault();
            if (item == null)
            {
                throw new EntityNotFoundException(
                    "stock item in store " + storeId + " of product " + productId + " could not be found");
            }
            item.Stock = stock;
            _context.SaveChanges();
        }


        public double GetProfitOfMonth(int storeId, int month, int year)
        {
            Store store = GetStore(storeId);
            var sales = _context.Sales
                .Where(sale =>
                    sale.StoreId == storeId &&
                    sale.TimeStamp.Year == year &&
                    sale.TimeStamp.Month == month
                )
                .Select(sale => sale.Id)
                .ToArray();

            double profit = 0;
            foreach (var sale in sales)
            {
                profit += _context.SaleElements
                    .Where(element => element.SaleId == sale)
                    .Include(element => element.Product)
                    .Select(element => element.Product.SalePrice * element.Amount)
                    .Aggregate((x, y) => x + y);
            }
            return profit;
        }

        public Statistic GetProfitOfYear(int storeId, int year)
        {
            var dataset = new List<double>();
            for (int month = 1; month <= 12; month++)
            {
                dataset.Add(GetProfitOfMonth(storeId, year, month));
            }
            return new Statistic{ Key = year, Dataset = dataset.ToArray() };
        }

        public IEnumerable<Statistic> GetProfit(int storeId)
        {
            var first = _context.Sales
                .Where(sale => sale.StoreId == storeId)
                .OrderBy(sale => sale.TimeStamp.Year)
                .Select(sale => sale.TimeStamp.Year)
                .FirstOrDefault();

            var now = DateTime.Now.Year;

            var statistics = new List<Statistic>();
            for (int year = first; year <= now; year++)
            {
                statistics.Add(GetProfitOfYear(storeId, year));
            }
            return statistics;
        }
    }
}
