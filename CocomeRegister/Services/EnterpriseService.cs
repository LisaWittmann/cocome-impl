using System.Linq;
using System.Collections.Generic;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using Microsoft.EntityFrameworkCore;
using CocomeStore.Models.Transfer;
using CocomeStore.Models.Database;
using CocomeStore.Services.Mapping;

namespace CocomeStore.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class EnterpriseService : IEnterpriseService
    {
        private CocomeDbContext _context;
        private IModelMapper _mapper;

        public EnterpriseService(
            CocomeDbContext context,
            IModelMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productTO"></param>
        public void CreateProduct(ProductTO productTO)
        {
            Product product = _mapper.CreateProduct(productTO);
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public void CreateProvider(Provider provider)
        {
            _context.Providers.Add(provider);
            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        public void CreateStore(Store store)
        {
            _context.Stores.Add(store);
            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderTO> GetAllOrders()
        { 
           return _context.Orders
                .Include(order => order.Store)
                .Include(order => order.Provider)
                .AsEnumerable()
                .GroupJoin(_context.OrderElements
                    .Include(element => element.Product),
                    order => order.Id,
                    element => element.OrderId,
                    (order, elements) =>
                        _mapper.CreateOrderTO(order, elements.AsEnumerable()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductTO> GetAllProducts()
        {
            return _context.Products
                .Include(product => product.Provider)
                .Select(product => _mapper.CreateProductTO(product));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Provider> GetAllProvider()
        {
            return _context.Providers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StockItem> GetAllStockItems()
        {
            return _context.StockItems
                .Include(item => item.Product)
                .Include(item => item.Store);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Store> GetAllStores()
        {
            return _context.Stores;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productTO"></param>
        public void UpdateProduct(int productId, ProductTO productTO)
        {
            Product product = _context.Products.Find(productId);
            if (product == null)
            {
                throw new EntityNotFoundException(
                    "product with id " + productId + " could not be found");
            }

            _mapper.UpdateProduct(product, productTO);
            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="provider"></param>
        public void UpdateProvider(int providerId, Provider provider)
        {
            Provider contextProvider = _context.Providers.Find(providerId);
            if (contextProvider == null)
            {
                throw new EntityNotFoundException(
                    "provider with id " + providerId + " could not be found");
            }

            contextProvider.Name = provider.Name;
            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="store"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<Store> GetStores(int productId)
        {
            return _context.StockItems
                .Include(item => item.Store)
                .Where(item => item.ProductId == productId)
                .Select(item => item.Store)
                .ToHashSet();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="productId"></param>
        public void AddToStock(int storeId, int productId)
        {
            Store store = _context.Stores.Find(storeId);
            Product product = _context.Products.Find(productId);
            if (store == null)
            {
                throw new EntityNotFoundException(
                    "store with id " + storeId + " could not be found");
            }
            if (product == null)
            {
                throw new EntityNotFoundException(
                    "product with id " + productId + " could not be found");
            }

            _context.StockItems.Add(new() { ProductId = productId, StoreId = storeId, Stock = 0 });
            _context.SaveChanges();
        }
    }
}
