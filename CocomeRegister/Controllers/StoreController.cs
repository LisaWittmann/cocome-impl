using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CocomeStore.Services;
using CocomeStore.Exceptions;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;
        private readonly IStoreService _service;

        public StoreController(
            ILogger<StoreController> logger,
            IStoreService service
        )
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Store>> GetAllStores()
        {
            return _service.GetAllStores().ToArray();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Store> GetStore(int id)
        {
            try
            {
                _logger.LogInformation("requesting store by id {}", id);
                return _service.GetStore(id);
            }
            catch(EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("inventory/{id}")]
        public ActionResult<IEnumerable<StockItem>> GetInventory(int id)
        {
            _logger.LogInformation("requesting inventory of store {}", id);
            return _service.GetInventory(id).ToArray();
            
        }

        [HttpGet]
        [Route("orders/{id}")]
        public ActionResult<IEnumerable<Order>> GetOrders(int id)
        {
            _logger.LogInformation("requesting orders of store {}", id);
            return _service.GetOrders(id).ToArray();
        }

        [HttpPost]
        [Route("create-order/{id}")]
        public ActionResult<IEnumerable<Order>> PlaceOrder(int id, IEnumerable<OrderElement> elements)
        {
            _logger.LogInformation("place new order for store {}", id);
            try
            {
                _service.PlaceOrder(id, elements);
                return _service.GetOrders(id).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
           
        }

        [HttpPost]
        [Route("close-order/{id}")]
        public ActionResult<IEnumerable<Order>> CloseOrder(int id, int orderId)
        {
            try
            {
                _logger.LogInformation("close order with id {} for store {}", orderId, id);
                _service.CloseOrder(id, orderId);
                return _service.GetOrders(id).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("create-product/{id}")]
        public ActionResult<IEnumerable<StockItem>> CreateProduct(int id, Product product)
        {
            try
            {
                _logger.LogInformation("adding new product to store {}", id);
                _service.CreateProduct(id, product);
                return _service.GetInventory(id).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
