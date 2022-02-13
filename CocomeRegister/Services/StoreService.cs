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
            Store store = _context.Stores.Find(1);
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

            if (order.Store.Id != storeId)
            {
                throw new CrossAccessException("no access to order " + orderId);
            }

            order.Closed = true;
            _context.SaveChanges();
        }


        public void PlaceOrder(int storeId, IEnumerable<OrderElementTO> elements)
        {
  
            Store store = _context.Stores.Find(storeId);
            if (store == null)
            {
                throw new EntityNotFoundException("store with id " + storeId + " could not be found");
            }

            // TODO: get provider by products
            Provider provider = _context.Providers.First();
            DateTime dateTime = DateTime.Now;

            Order order = new Order {
                StoreId = store.Id,
                ProviderId = provider.Id,
                PlacingDate = dateTime,
                Closed = false,
                Delivered = false
            };
            _context.Orders.Add(order);

            foreach (var element in elements)
            {
                _context.OrderElements.Add(_mapper.CreateOrderElement(order, element));
            }
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


        public void CreateProduct(int storeId, ProductTO productTO)
        {
            Product product = _mapper.CreateProduct(productTO);
            StockItem item = new StockItem { Product = product, Stock = 0, StoreId = storeId };
  
            _context.StockItems.Add(item);
            _context.SaveChanges();
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


        public float GetProfitOfMonth(int storeId, int month, int year)
        {
            Store store = GetStore(storeId);
            var sales = _context.Sales
                .Where(sale =>
                    sale.Store.Id == storeId &&
                    sale.TimeStamp.Year == year &&
                    sale.TimeStamp.Month == month
                );

            float profit = 0;
            foreach (var sale in sales)
            {
                var profits = _context.SaleElements
                    .Where(element => element.Sale.Id == sale.Id)
                    .Select(element => element.Product.SalePrice - element.Product.Price);
                profit += profits.Aggregate((x, y) => (x + y));
            }
            return profit;
        }
    }
}
