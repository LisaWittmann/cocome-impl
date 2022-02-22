using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Exceptions;
using Microsoft.EntityFrameworkCore;
using CocomeStore.Models.Transfer;
using CocomeStore.Models.Database;
using CocomeStore.Services.Mapping;

namespace CocomeStore.Services
{
    /// <summary>
    /// class <c>StoreService</c> implenents <see cref="IStoreService"/>
    /// and provides store intern functionalities
    /// </summary>
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

        /// <summary>
        /// method <c>GetStore</c> returns the store entry in the database with
        /// the given id
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <returns>store entry from database</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public Store GetStore(int storeId)
        {
            Store store = _context.Stores.Find(storeId);
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
            return _context.Orders
                .Where(order => order.StoreId == storeId &&
                    order.DeliveringDate == DateTime.MinValue)
                .Include(order => order.Store)
                .Include(order => order.Provider)
                .AsEnumerable()
                .GroupJoin(_context.OrderElements
                    .Include(element => element.Product),
                    order => order.Id,
                    element => element.OrderId,
                    (order, elements) => _mapper.CreateOrderTO(order, elements.AsEnumerable()));
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
