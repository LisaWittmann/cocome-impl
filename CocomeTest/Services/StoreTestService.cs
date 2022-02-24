using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Exceptions;
using Microsoft.EntityFrameworkCore;
using CocomeStore.Models.Transfer;
using CocomeStore.Models.Database;
using CocomeStore.Services.Mapping;
using CocomeStore.Services;

namespace CocomeStore.Test.Services
{
    /// <summary>
    /// class <c>StoreService</c> implenents <see cref="IStoreService"/>
    /// and provides store intern functionalities
    /// </summary>
    public class StoreTestService : IStoreService
    {
        private readonly List<Store> _store;
        private readonly List<Provider> _provider;
        private readonly List<OrderTO> _order;

        public StoreTestService()
        {
            _store = new List<Store>()
            {
                new Store() { Id = 1, City = "Teststadt", Name="Teststore1", PostalCode=1234},
                new Store() { Id = 2, City = "Teststadt", Name="Teststore2", PostalCode=1234},
                new Store() { Id = 3, City = "Teststadt", Name="Teststore3", PostalCode=1234},
                new Store() { Id = 4, City = "Teststadt", Name="Teststore4", PostalCode=1234}
            };
            _provider = new List<Provider>() {
                new Provider() {Id = 1, Name = "Testprovider1"},
                new Provider() { Id = 2, Name = "Testprovider2" }
            };
            _order = new List<OrderTO>()
            {
                new OrderTO() { Id = 1, Closed=false, Store=_store[0], PlacingDate=new DateTime().AddDays(-1), Provider=_provider[0]},
            };
        }

        /// <summary>
        /// method <c>GetStore</c> returns the store entry in the database with
        /// the given id
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <returns>store entry from database</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public Store GetStore(int storeId)
        {
            Store store = null;
            foreach (Store item in _store)
            {
                if (item.Id == storeId) { store = item};
            }
            if (store == null)
            {
                throw new EntityNotFoundException("store with id " + storeId + " could not be found");
            }
            return store;
        }

        /// <summary>
        /// method <c>GetOrders</c> returns the open orders related to the given
        /// store converted as transfer objects
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <returns>list of order transfer objects</returns>
        public IEnumerable<OrderTO> GetOrders(int storeId)
        {
            List<OrderTO> orders = new List<OrderTO>();
            foreach (OrderTO item in _order)
            {
                if (item.Store.Id == storeId) { orders.Add(item); }
            }
            return orders;
        }

        /// <summary>
        /// method <c>CloseOrder</c> provides the funcionality to mark a stores
        /// order as delivered and update the orders deliveringDate to the current
        /// timestamp
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="orderId">unique identitfier of the order</param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="CrossAccessException"></exception>
        public void CloseOrder(int storeId, int orderId)
        {
            OrderTO order = null;

            foreach (OrderTO item in _order)
            {
                if (item.Store.Id == storeId && item.Id == orderId) { order = item; }
            }

            if (order == null)
            {
                throw new EntityNotFoundException("order with id " + orderId + " could not be found");
            }

            if (order.Store.Id != storeId)
            {
                throw new CrossAccessException("no access to order " + orderId);
            }

            var orderElements = _context.OrderElements
                .Where(element => element.OrderId == orderId)
                .Include(element => element.Product);

            foreach (var element in orderElements)
            {
                StockItem item = _context.StockItems
                    .Where(item => item.ProductId == element.ProductId && item.StoreId == storeId)
                    .SingleOrDefault();
                item.Stock += element.Amount;
            };

            order.DeliveringDate = DateTime.Now;
            _context.SaveChanges();
        }

        /// <summary>
        /// method <c>PlaceOrder</c> provides the functionality to create a new
        /// order and add it to the database
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="elements">order transfer elements of the new order</param>
        /// <exception cref="EntityNotFoundException"></exception>
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
                    DeliveringDate = DateTime.MinValue
                };

                var orderElements = element.ToArray()
                    .Select(element => _mapper.CreateOrderElement(order, element))
                    .ToArray();

                _context.AddRange(orderElements);
            };
            _context.SaveChanges();
        }


        /// <summary>
        /// method <c>GetInventory</c> returns all stockitems entries related to
        /// the given store
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <returns>list of stock items</returns>
        public IEnumerable<StockItem> GetInventory(int storeId)
        {
            return _context.StockItems
                .Where(item => item.Store.Id == storeId)
                .Include(item => item.Store)
                .Include(item => item.Product)
                .ThenInclude(product => product.Provider);
        }

        /// <summary>
        /// method <c>GetProduct</c> returns a product of the inventoy of the
        /// given store with the given product id
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="productId">unique identitfier of the product</param>
        /// <returns>product entry converted to transfer object</returns>
        /// <exception cref="CrossAccessException"></exception>
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

        /// <summary>
        /// method <c>UpdateProduct</c> provides the funtionality to update
        /// a product entry in the database by the transfer objects id and apply
        /// the transfer objects data to the entry object
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="productTO">transfer object containing the new data</param>
        public void UpdateProduct(int storeId, ProductTO productTO)
        {
            Product product = _context.Products.Find(productTO.Id);
            _mapper.UpdateProduct(product, productTO);
            _context.SaveChanges();
        }
    }
}
