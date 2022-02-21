using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CocomeStore.Services;
using CocomeStore.Exceptions;
using CocomeStore.Models.Transfer;
using Microsoft.AspNetCore.Authorization;

namespace CocomeStore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Authorize(Policy = "store")]
    [Route("api/[controller]")]
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;
        private readonly IStoreService _service;
        private readonly IDatabaseStatistics _statistics;

        public StoreController(
            ILogger<StoreController> logger,
            IStoreService service,
            IDatabaseStatistics statistics
        )
        {
            _service = service;
            _logger = logger;
            _statistics = statistics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Store> GetAllStores()
        {
            return _service.GetAllStores().ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("profit/{id}")]
        public ActionResult<IEnumerable<Statistic>> GetProfit(int id)
        {
            try
            {
                return _statistics.GetStoreProfit(id).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("inventory/{id}")]
        public ActionResult<IEnumerable<StockItem>> GetInventory(int id)
        {
            _logger.LogInformation("requesting inventory of store {}", id);
            return _service.GetInventory(id).ToArray();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("orders/{id}")]
        public ActionResult<IEnumerable<OrderTO>> GetOrders(int id)
        {
            _logger.LogInformation("requesting orders of store {}", id);
            return _service.GetOrders(id).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("close-order/{id}")]
        public ActionResult<IEnumerable<OrderTO>> CloseOrder(int id, OrderTO orderTO)
        {
            try
            {
                _logger.LogInformation("close order with id {} for store {}", orderTO.Id, id);
                _service.CloseOrder(id, orderTO.Id);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/profit/{year}")]
        public ActionResult<Statistic> GetProfit(int id, int year)
        {
            try
            {
                return _statistics.GetProfitOfYear(id, year);
            }
            catch(EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

    }
}
