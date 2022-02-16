using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CocomeStore.Services;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;
using Microsoft.AspNetCore.Authorization;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Authorize(Policy = "enterprise")]
    [Route("api/[controller]")]
    public class EnterpriseController : Controller
    {
        private readonly ILogger<EnterpriseController> _logger;
        private readonly IEnterpriseService _service;
        private readonly IDatabaseStatistics _statistics;

        public EnterpriseController(
            ILogger<EnterpriseController> logger,
            IEnterpriseService service,
            IDatabaseStatistics statistics
        )
        {
            _logger = logger;
            _service = service;
            _statistics = statistics;
        }

        [HttpGet]
        [Route("orders")]
        public ActionResult<IEnumerable<OrderTO>> GetAllOrders()
        {
            return _service.GetAllOrders().ToArray();
        }

        [HttpGet]
        [Route("inventories")]
        public ActionResult<IEnumerable<StockItem>> GetAllStockItems()
        {
            return _service.GetAllStockItems().ToArray();
        }

        [HttpGet]
        [Route("stores")]
        public ActionResult<IEnumerable<Store>> GetAllStores()
        {
            return _service.GetAllStores().ToArray();
        }

        [HttpGet]
        [Route("provider")]
        public ActionResult<IEnumerable<Provider>> GetAllProvider()
        {
            return _service.GetAllProvider().ToArray();
        }

        [HttpGet]
        [Route("products")]
        public ActionResult<IEnumerable<ProductTO>> GetAllProducts()
        {
            return _service.GetAllProducts().ToArray();
        }

        [HttpGet]
        [Route("product/{id}/stores")]
        public ActionResult<IEnumerable<Store>> GetStores(int id)
        {
            return _service.GetStores(id).ToArray();
        }

        [HttpPost]
        [Route("create-product")]
        public ActionResult<IEnumerable<ProductTO>> CreateProduct(ProductTO product)
        {
            try
            {
                _service.CreateProduct(product);
                return _service.GetAllProducts().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update-product/{id}")]
        public ActionResult<IEnumerable<ProductTO>> UpdateProduct(int id, ProductTO product)
        {
            try
            {
                _service.UpdateProduct(id, product);
                return _service.GetAllProducts().ToArray();
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
        [Route("create-store")]
        public ActionResult<IEnumerable<Store>> CreateStore(Store store)
        {
            try
            {
                _service.CreateStore(store);
                return _service.GetAllStores().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update-store/{id}")]
        public ActionResult<IEnumerable<Store>> UpdateStore(int id, Store store)
        {
            try
            {
                _service.UpdateStore(id, store);
                return _service.GetAllStores().ToArray();
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
        [Route("create-provider")]
        public ActionResult<IEnumerable<Provider>> CreateProvider(Provider provider)
        {
            try
            {
                _service.CreateProvider(provider);
                return _service.GetAllProvider().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update-provider/{id}")]
        public ActionResult<IEnumerable<Provider>> UpdateProvider(int id, Provider provider)
        {
            try
            {
                _service.UpdateProvider(id, provider);
                return _service.GetAllProvider().ToArray();
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
        [Route("create-stock/{id}")]
        public ActionResult<IEnumerable<Store>> CreateStock(int id, ProductTO product)
        {
            try
            {
                _logger.LogInformation("adding product {} to store {}", product.Id, id);
                _service.addToStock(id, product.Id);
                return _service.GetStores(product.Id).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("store-reports")]
        public IEnumerable<Statistic> GetStoreStatistic()
        {
            return _statistics.GetLatestProfit().ToArray();
        }

        [HttpGet]
        [Route("provider-reports")]
        public IEnumerable<Statistic> GetProvidersStatistic()
        {
            return _statistics.GetProvidersStatistic().ToArray();
        }


        [HttpGet]
        [Route("provider-report/{id}")]
        public ActionResult<Statistic> GetProviderStatistic(int id)
        {
            try
            {
                return _statistics.GetProviderStatistic(id);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }
    }
}
