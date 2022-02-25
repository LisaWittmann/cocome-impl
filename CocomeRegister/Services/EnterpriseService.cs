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
    /// class <c>EnterpriseServices</c> implements <see cref="IEnterpriseService"/>
    /// and provides functionalities for the enterprise administrator
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
        /// method <c>CreateProduct</c> creates a new product
        /// entry in the database out of the incomming information
        /// </summary>
        /// <param name="productTO">
        /// transfer object containing the product information
        /// </param>
        public ProductTO CreateProduct(ProductTO productTO)
        {
            Product product = _mapper.CreateProduct(productTO);
            _context.Products.Add(product);
            _context.SaveChanges();
            return _mapper.CreateProductTO(product);
        }

        /// <summary>
        /// method <c>CreateProvider</c> creates a new provider
        /// entry in the databse out of the incomming information
        /// </summary>
        /// <param name="provider">
        /// object containing the providers information
        /// </param>
        public Provider CreateProvider(Provider provider)
        {
            _context.Providers.Add(provider);
            _context.SaveChanges();
            return provider;
        }

        /// <summary>
        /// method <c>CreateStore</c> creates a new store
        /// entry in the database out of the incomming information
        /// </summary>
        /// <param name="store">
        /// object containing the stores information
        /// </param>
        public Store CreateStore(Store store)
        {
            _context.Stores.Add(store);
            _context.SaveChanges();
            return store;
        }

        /// <summary>
        /// method <c>GetAllProducts</c> returns all
        /// product entries in database as transfer object
        /// including related objects
        /// </summary>
        /// <returns>
        /// enumerable product entries as transfer objects
        /// </returns>
        public IEnumerable<ProductTO> GetAllProducts()
        {
            return _context.Products
                .Include(product => product.Provider)
                .Select(product => _mapper.CreateProductTO(product));
        }

        /// <summary>
        /// method <c>GetAllProviders</c> returns all
        /// provider entries from database
        /// </summary>
        /// <returns>
        /// enumerable provider entires
        /// </returns>
        public IEnumerable<Provider> GetAllProvider()
        {
            return _context.Providers;
        }

        /// <summary>
        /// method <c>GetAllStores</c> returns all
        /// store entries from database
        /// </summary>
        /// <returns>
        /// enumerable store entries
        /// </returns>
        public IEnumerable<Store> GetAllStores()
        {
            return _context.Stores;
        }

        /// <summary>
        /// method <c>UpdateProduct</c> provides funtionality
        /// to modifiy a product entry in the databse
        /// </summary>
        /// <param name="productId">
        /// unique identifier of the product to perform changes on
        /// </param>
        /// <param name="productTO">
        /// object containing the modified informations
        /// </param>
        /// <exception cref="EntityNotFoundException"></exception>
        public ProductTO UpdateProduct(int productId, ProductTO productTO)
        {
            Product product = _context.Products.Find(productId);
            if (product == null)
            {
                throw new EntityNotFoundException(
                    "product with id " + productId + " could not be found");
            }

            _mapper.UpdateProduct(product, productTO);
            _context.SaveChanges();
            return productTO;
        }

        /// <summary>
        /// method <c>UpdateProvider</c> provides functionality
        /// to modify a provider entry in the database
        /// </summary>
        /// <param name="providerId">
        /// unique identifier of the provider to perform changes on
        /// </param>
        /// <param name="provider">
        /// object containing the modified informations
        /// </param>
        /// <exception cref="EntityNotFoundException"></exception>
        public Provider UpdateProvider(int providerId, Provider provider)
        {
            Provider contextProvider = _context.Providers.Find(providerId);
            if (contextProvider == null)
            {
                throw new EntityNotFoundException(
                    "provider with id " + providerId + " could not be found");
            }

            contextProvider.Name = provider.Name;
            _context.SaveChanges();
            return contextProvider;
        }

        /// <summary>
        /// method <c>UpdateStore</c> provides functionality
        /// to modify a store entry in the database
        /// </summary>
        /// <param name="storeId">
        /// unique identifier of the store to perform changes on
        /// </param>
        /// <param name="store">
        /// object containing the modified informations
        /// </param>
        /// <exception cref="EntityNotFoundException"></exception>
        public Store UpdateStore(int storeId, Store store)
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
            return contextStore;
        }

        /// <summary>
        /// method <c>GetStores</c> returns all stores
        /// that sell the product with the given id
        /// </summary>
        /// <param name="productId">
        /// unique identifier of the product
        /// </param>
        /// <returns>
        /// enumerable store entries without duplications
        /// </returns>
        public IEnumerable<Store> GetStores(int productId)
        {
            return _context.StockItems
                .Include(item => item.Store)
                .Where(item => item.ProductId == productId)
                .Select(item => item.Store)
                .ToHashSet();
        }

        /// <summary>
        /// method <c>AddToStock</c> creates a new stockitem entry
        /// in the database for the given store of the given product
        /// </summary>
        /// <param name="storeId">
        /// unique identifier of the store
        /// </param>
        /// <param name="productId">
        /// unique identifier ot the product
        /// </param>
        /// <exception cref="EntityNotFoundException"></exception>
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

            var registred = _context.StockItems
                .Where(item => item.ProductId == productId && item.StoreId == storeId)
                .ToArray().Length > 0;
            if (registred)
            {
                return;
            }

            _context.StockItems.Add(new() { ProductId = productId, StoreId = storeId, Stock = 0 });
            _context.SaveChanges();
        }
    }
}
