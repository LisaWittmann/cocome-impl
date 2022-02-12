using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.Extensions.Logging;
using CocomeStore.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Services
{
    public class StoreService : IStoreService
    {
        private readonly ILogger<StoreService> _logger;
        private CocomeDbContext _context;
        private Random _random;

        public StoreService(ILogger<StoreService> logger, CocomeDbContext context)
        {
            _logger = logger;
            _context = context;
            _random = new Random();
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

        public IEnumerable<Order> GetOrders(int storeId)
        {
            return _context.Orders
                    .Where(order => order.Store.Id == storeId)
                    .Include(order => order.Store)
                    .Include(order => order.Provider);
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

        public void PlaceOrder(int storeId, IEnumerable<OrderElement> elements)
        {
            DateTime dateTime = DateTime.Now;
            Store store = GetStore(storeId);
            Provider provider = _context.Providers.First();

            foreach (OrderElement element in elements)
            {
                _context.Orders.Add(new Order
                {
                    Id = _random.Next(),
                    orderElements = new[] {
                        new OrderElement { Product = element.Product, Amount = element.Amount }
                    },
                    Store = store,
                    Provider = provider,
                    PlacingDate = dateTime,
                    DeliveringDate = dateTime,
                    Closed = false,
                    Delivered = false,
                });
            }
            _context.SaveChanges();
        }

        public IEnumerable<StockItem> GetInventory(int storeId)
        {
            return _context.StockItems
                    .Where(item => item.Store.Id == storeId)
                    .Include(item => item.Product)
                    .Include(item => item.Store);
        }

        public void CreateProduct(int storeId, Product product)
        {
            Store store = GetStore(storeId);
            StockItem item = new StockItem { Product = product, Stock = 0, Store = store };
     
            _context.Products.Add(product);
            _context.StockItems.Add(item);
            _context.SaveChanges();
        }

        public void UpdateProduct(int storeId, Product product)
        {
            Product savedProduct = _context.Products.Find(product.Id);
            savedProduct.Name = product.Name;
            savedProduct.Price = product.Price;
            savedProduct.SalePrice = product.SalePrice;
            savedProduct.ImageUrl = product.ImageUrl;
            _context.SaveChanges();
        }

        public void UpdateStock(int storeId, int productId, int stock)
        {
            StockItem item = _context.StockItems
                                .Where(item => item.Store.Id == storeId && item.Product.Id == productId)
                                .First();
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
                var profits = sale.SaleElements.Select(element => element.Product.SalePrice - element.Product.Price);
                profit += profits.Aggregate((x, y) => (x + y));
            }
            return profit;
        }

        public IEnumerable<float> GetProfitOfYear(int storeId, int year)
        {
            var profits = new List<float>();
            for (int month = 1; month <= 12; month++)
            {
                profits.Add(GetProfitOfMonth(storeId, month, year));
            }
            return profits;
        }
    }
}
