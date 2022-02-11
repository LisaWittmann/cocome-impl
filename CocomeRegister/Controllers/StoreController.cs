using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;
        private CocomeDbContext _context;

        public StoreController(ILogger<StoreController> logger, CocomeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Store> GetAllStores()
        {
            return _context.Stores;
        }

        [HttpGet]
        [Route("{id}")]
        public Store GetStore(int id)
        {
            _logger.LogInformation("requesting store by id {}", id);
            Store store = _context.Stores.Find(id);
            if (store == null)
            {
                NotFound();
            }
            return store;
        }

        [HttpGet]
        [Route("inventory")]
        public IEnumerable<StockItem> GetAllStockItems()
        {
            return _context.StockItems
                    .Include(item => item.Product)
                    .Include(item => item.Store);
        }

        [HttpGet]
        [Route("inventory/{id}")]
        public IEnumerable<StockItem> GetInventory(int id)
        {
            _logger.LogInformation("requesting inventory of store {}", id);
            return _context.StockItems.Where(item => item.Store.Id == id)
                    .Include(item => item.Product)
                    .Include(item => item.Store);
        }

        [HttpGet]
        [Route("orders")]
        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                    .Include(order => order.Product)
                    .Include(order => order.Store); ;
        }

        [HttpGet]
        [Route("orders/{id}")]
        public IEnumerable<Order> GetOrders(int id)
        {
            _logger.LogInformation("requesting orders of store {}", id);
            return _context.Orders.Where(order => order.Store.Id == id)
                    .Include(order => order.Product)
                    .Include(order => order.Store);
        }

        [HttpPost]
        [Route("create-order/{id}")]
        public IEnumerable<Order> PlaceOrder(int id, IDictionary<Product, int> products)
        {
            _logger.LogInformation("place new order for store {}", id);
            DateTime dateTime = DateTime.Now;
            Store store = GetStore(id);
            try
            {
               foreach(var item in products)
                {
                    _context.Orders.Add(new Order { Product = item.Key, Amount = item.Value, Store = store, Closed = false, Delivered = false, PlacingDate = dateTime });
                }
                _context.SaveChanges();
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                BadRequest();
            }
            return _context.Orders.Where(item => item.Store.Id == id)
                    .Include(order => order.Product)
                    .Include(order => order.Store);
        }

        [HttpPost]
        [Route("close-order/{id}")]
        public IEnumerable<Order> CloseOrder(int id, int orderId)
        {
            _logger.LogInformation("close order with id {} for store {}", orderId, id);
            Order order = _context.Orders.Find(orderId);
            if (order == null)
            {
                NotFound();
            }

            try
            {
                order.Closed = true;
                _context.SaveChanges();
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                BadRequest();
            }
            return _context.Orders.Where(order => order.Store.Id == id)
                    .Include(order => order.Product)
                    .Include(order => order.Store);
        }


        [HttpPost]
        [Route("create-product/{id}")]
        public StockItem CreateProduct(int id, Product product)
        {
            _logger.LogInformation("adding new product to store {}", id);
            Store store = GetStore(id);
            StockItem item = new StockItem { Product = product, Stock = 0, Store = store };
            try
            {
                _context.Products.Add(product);
                _context.StockItems.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                BadRequest();
            }

            return item;
        }

        [HttpPost]
        [Route("update-inventory/{id}")]
        public IEnumerable<StockItem> UpdateInventory(int id, IList<StockItem> inventory)
        {
            _logger.LogInformation("update inventory for store {}", id);
            // TODO: Update Inventory
            return _context.StockItems.Where(item => item.Store.Id == id)
                    .Include(item => item.Product)
                    .Include(item => item.Store);
        }
    }
}
