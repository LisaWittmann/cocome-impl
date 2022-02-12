using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Services
{
    public class EnterpriseService : IEnterpriseService
    {
        private CocomeDbContext _context;

        public EnterpriseService(CocomeDbContext context)
        {
            _context = context;
        }

        public void CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void CreateProvider(Provider provider)
        {
            _context.Providers.Add(provider);
            _context.SaveChanges();
        }

        public void CreateStore(Store store)
        {
            _context.Stores.Add(store);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                    .Include(order => order.Store)
                    .Include(order => order.Provider);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products;
        }

        public IEnumerable<Provider> GetAllProvider()
        {
            return _context.Providers;
        }

        public IEnumerable<StockItem> GetAllStockItems()
        {
            return _context.StockItems
                    .Include(item => item.Product)
                    .Include(item => item.Store);
        }

        public IEnumerable<Store> GetAllStores()
        {
            return _context.Stores;
        }

        public IEnumerable<TimeSpan> GetDeliverySpans(int providerId)
        {
            Provider provider = _context.Providers.Find(providerId);
            if (provider == null)
            {
                throw new EntityNotFoundException(
                   "provider with id " + providerId + " could not be found");
            }

            IEnumerable<Order> providerOrders = _context.Orders
                .Where(order => order.Provider.Id == providerId);

            var timeSpans = new List<TimeSpan>();
            foreach (var order in providerOrders)
            {
                if (order.Delivered)
                {
                    TimeSpan span = order.DeliveringDate - order.PlacingDate;
                    timeSpans.Add(span);
                }
            }

            return timeSpans;
        }

        public void UpdateProduct(int productId, Product product)
        {
            Product contextProduct = _context.Products.Find(productId);
            if (contextProduct == null)
            {
                throw new EntityNotFoundException(
                    "product with id " + productId + " could not be found");
            }

            contextProduct.Name = product.Name;
            contextProduct.Price = product.Price;
            contextProduct.SalePrice = product.SalePrice;
            contextProduct.Description = product.Description;
            contextProduct.ImageUrl = product.ImageUrl;
            _context.SaveChanges();
        }

        public void UpdateProvider(int providerId, Provider provider)
        {
            Provider contextProvider = _context.Providers.Find(providerId);
            if (contextProvider == null)
            {
                throw new EntityNotFoundException(
                    "provider with id " + providerId + " could not be found");
            }

            contextProvider.Name = provider.Name;
            contextProvider.Products = provider.Products;
            _context.SaveChanges();
        }

        public void UpdateStore(int storeId, Store store)
        {
            Store contextStore = _context.Stores.Find(storeId);
            if (store == null)
            {
                throw new EntityNotFoundException(
                    "store with id " + storeId + " could not be found");
            }

            contextStore.Name = store.Name;
            contextStore.City = store.City;
            contextStore.PostalCode = store.PostalCode;
            _context.SaveChanges();
        }
    }
}
