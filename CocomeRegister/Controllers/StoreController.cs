using System;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;
        private static Random random = new Random();

        CocomeDbContext _context;

        private static Store testStore = new Store { Id = random.Next(), Name = "TestFiliale Lisa", City = "Bacharach", PostalCode = 55422 };
        private static IList<StockItem> testInventory = new List<StockItem>()
        {
            new StockItem { Id = random.Next(), Product = (new Product { Id = random.Next(), Name = "Salatgurke", SalePrice = 0.59F, Price = 0.10F}), Stock = random.Next(100), Store = testStore },
            new StockItem { Id = random.Next(), Product = (new Product { Id = random.Next(), Name = "Endiviensalat", SalePrice = 0.99F, Price = 0.17F}), Stock = random.Next(100), Store = testStore },
            new StockItem { Id = random.Next(), Product = (new Product { Id = random.Next(), Name = "Kräuterbaguette", SalePrice = 1.59F, Price = 0.99F}), Stock = random.Next(100), Store = testStore },
            new StockItem { Id = random.Next(), Product = (new Product { Id = random.Next(), Name = "Schokoriegel", SalePrice = 0.99F, Price = 0.50F}), Stock = random.Next(100), Store = testStore },
            new StockItem { Id = random.Next(), Product = (new Product { Id = random.Next(), Name = "Papaya", SalePrice = 2.15F, Price = 1.15F}), Stock = random.Next(100), Store = testStore },
            new StockItem { Id = random.Next(), Product = (new Product { Id = random.Next(), Name = "Kartoffeln", SalePrice = 1.99F, Price = 0.99F}), Stock = random.Next(100), Store = testStore },
            new StockItem { Id = random.Next(), Product = (new Product { Id = random.Next(), Name = "Eistee Zitrone", SalePrice = 1.59F, Price = 0.75F}), Stock = random.Next(100), Store = testStore },
            new StockItem { Id = random.Next(), Product = (new Product { Id = random.Next(), Name = "Schlagsahne", SalePrice = 0.29F, Price = 0.15F}), Stock = random.Next(100), Store = testStore },
        };

        public StoreController(ILogger<StoreController> logger, CocomeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public Store GetStore(int id)
        {
            _logger.LogInformation("requesting store by id {}", id);
            try
            {
                Store store = _context.Find<Store>(id);
                _logger.LogInformation("Found store: {}", store);
                return store;
            } catch (Exception ex)
            {
                _logger.LogError("Could not find Store with id {}. Returning testData.", id);
                return testStore;
            }
            
        }

        [HttpGet]
        [Route("{id}/product/{productId}")]
        public Product GetProduct(int id, int productId)
        {
            try
            {
                return _context.Find<Product>(productId);
            } catch 
            {
                Product product = null;
                foreach (StockItem item in testInventory)
                {
                    if (item.Product.Id == productId)
                    {
                        product = item.Product;
                    }
                }
                return product;
            }
            
        }

        [HttpGet]
        [Route("inventory/{id}")]
        public IEnumerable<StockItem> GetInventory(int id)
        {
            _logger.LogInformation("requesting inventory of store {}", id);
            return testInventory;
        }

        [HttpGet]
        [Route("orders/{id}")]
        public IEnumerable<Order> GetOrders(int id)
        {
            _logger.LogInformation("requesting orders of store {}", id);
            IEnumerable<Order> storeOrders = new List<Order>();
            return storeOrders;
        }

        [HttpPost]
        [Route("create-product/{id}")]
        public StockItem CreateProduct(int id, Product product)
        {
            _logger.LogInformation("adding new product to store {}", id);
            StockItem item = new StockItem { Id = random.Next(), Product = product, Stock = 0, Store = testStore };
            try
            {
                _context.Add(item);
                _context.SaveChanges();
            } catch (Exception ex)
            {
                testInventory.Add(item);
            }
            
            return item;

        }

        [HttpPost]
        [Route("create-order/{id}")]
        public IEnumerable<Order> PlaceOrder(int id, IDictionary<Product, int> products)
        {
            _logger.LogInformation("place new order for store {}", id);
            IEnumerable<Order> storeOrders = new List<Order>();
            return storeOrders;
        }

        [HttpPost]
        [Route("close-order/{id}")]
        public IEnumerable<Order> CloseOrder(int id, int orderId)
        {
            _logger.LogInformation("close order with id {} for store {}", orderId, id);
            IEnumerable<Order> storeOrders = new List<Order>();
            return storeOrders;
        }

        [HttpPost]
        [Route("update-inventory/{id}")]
        public IEnumerable<StockItem> UpdateInventory(int id, IList<StockItem> inventory)
        {
            _logger.LogInformation("update inventory for store {}", id);
            testInventory = inventory;
            return testInventory;
        }
    }
}
