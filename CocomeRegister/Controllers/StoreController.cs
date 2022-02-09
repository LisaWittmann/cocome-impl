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

        private static Store testStore = new Store { Id = random.Next(250), Name = "Filiale Lisa", City = "Bacharach", PostalCode = 55422 };
        private static IEnumerable<StockItem> testInventory = new []
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

        public StoreController(ILogger<StoreController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public Store GetStore(int id)
        {
            _logger.LogInformation("requesting store by id {}", id);
            return testStore;
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
        public IEnumerable<StockItem> UpdateInventory(int id, IEnumerable<StockItem> inventory)
        {
            _logger.LogInformation("update inventory for store {}", id);
            testInventory = inventory;
            return testInventory;
        }
    }
}
