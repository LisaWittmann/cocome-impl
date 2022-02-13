using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CocomeStore.Services;
using CocomeStore.Exceptions;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : Controller
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
        public IEnumerable<Store> GetAllStores()
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
        [Route("{id}/product/{productId}")]
        public ActionResult<ProductTO> GetProduct(int id, int productId)
        {
            try
            {
                _logger.LogInformation("requesting product {} of store {}", productId, id);
                return _service.GetProduct(id, productId);
            }
            catch (CrossAccessException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict();
            }
        }

        [HttpPost]
        [Route("update-product/{id}")]
        public ActionResult<IEnumerable<StockItem>> UpdateProduct(int id, ProductTO productTO)
        {
            try
            {
                _logger.LogInformation("updating product {} from store {}", productTO.Name, id);
                _service.UpdateProduct(id, productTO);
            }
            catch (CrossAccessException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
            return _service.GetInventory(id).ToArray();
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
        public ActionResult<IEnumerable<OrderTO>> GetOrders(int id)
        {
            _logger.LogInformation("requesting orders of store {}", id);
            return _service.GetOrders(id).ToArray();
        }

        [HttpPost]
        [Route("create-order/{id}")]
        public ActionResult<IEnumerable<OrderTO>> PlaceOrder(int id, IEnumerable<OrderElementTO> elements)
        {
            try
            {
                _logger.LogInformation("place new order for store {}", id);
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
        public ActionResult<IEnumerable<OrderTO>> CloseOrder(int id, int orderId)
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
    }
}
