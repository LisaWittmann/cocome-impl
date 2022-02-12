using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CocomeStore.Services;
using CocomeStore.Exceptions;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;
        private CocomeDbContext _context;
        private IStoreService _service;
        private Random random = new Random();

        public StoreController(
            ILogger<StoreController> logger,
            IStoreService service,
            CocomeDbContext context
        )
        {
            _service = service;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Store> GetAllStores()
        {
            return _context.Stores.ToArray();
        }

        [HttpGet]
        [Route("{id}")]
        public Store GetStore(int id)
        {
            try
            {
                _logger.LogInformation("requesting store by id {}", id);
                return _service.GetStore(id);
            }
            catch(EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                NotFound();
            }
            return null;
        }

        [HttpGet]
        [Route("inventory/{id}")]
        public IEnumerable<StockItem> GetInventory(int id)
        {
            _logger.LogInformation("requesting inventory of store {}", id);
            return _service.GetInventory(id);
            
        }

        [HttpGet]
        [Route("orders/{id}")]
        public IEnumerable<Order> GetOrders(int id)
        {
            _logger.LogInformation("requesting orders of store {}", id);
            return _service.GetOrders(id);
        }

        [HttpPost]
        [Route("create-order/{id}")]
        public IEnumerable<Order> PlaceOrder(int id, IEnumerable<OrderElement> elements)
        {
            _logger.LogInformation("place new order for store {}", id);
            try
            {
                _service.PlaceOrder(id, elements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                BadRequest();
            }
            return _service.GetOrders(id);
        }

        [HttpPost]
        [Route("close-order/{id}")]
        public IEnumerable<Order> CloseOrder(int id, int orderId)
        {
            _logger.LogInformation("close order with id {} for store {}", orderId, id);

            try
            {
                _service.CloseOrder(id, orderId);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                BadRequest();
            }

            return _service.GetOrders(id);
        }


        [HttpPost]
        [Route("create-product/{id}")]
        public IEnumerable<StockItem> CreateProduct(int id, Product product)
        {
            _logger.LogInformation("adding new product to store {}", id);
 
            try
            {
                _service.CreateProduct(id, product);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                NotFound();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                BadRequest();
            }

            return _service.GetInventory(id); ;
        }
    }
}
