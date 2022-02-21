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
    /// 
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
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
        /// <param name="storeId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="orderId"></param>
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="elements"></param>
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public IEnumerable<StockItem> GetInventory(int storeId)
        {
            return _context.StockItems
                .Where(item => item.Store.Id == storeId)
                .Include(item => item.Store)
                .Include(item => item.Product)
                .ThenInclude(product => product.Provider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="productTO"></param>
        public void UpdateProduct(int storeId, ProductTO productTO)
        {
            Product product = _context.Products.Find(productTO.Id);
            _mapper.UpdateProduct(product, productTO);
            _context.SaveChanges();
        }
    }
}
