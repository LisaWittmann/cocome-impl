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
    /// <summary>
    /// class <c>EnterpriseController</c> provides REST endpoints for the
    /// enterprise application and requires authorization of enterprise policy
    /// </summary>
    [ApiController]
    [Authorize(Policy = "enterprise")]
    [Route("api/[controller]")]
    public class EnterpriseController : Controller
    {
        private readonly ILogger<EnterpriseController> _logger;
        private readonly IEnterpriseService _enterpriseService;
        private readonly IReportService _reportService;

        public EnterpriseController(
            ILogger<EnterpriseController> logger,
            IReportService reportService,
            IEnterpriseService enterpriseService
        )
        {
            _logger = logger;
            _reportService = reportService;
            _enterpriseService = enterpriseService;
        }

        /// <summary>
        /// method <c>GetAllOrders</c> is an http endpoint to request all related
        /// order entries to the given storeid
        /// </summary>
        /// <returns>
        /// list of orders as transfer objects
        /// </returns>
        [HttpGet]
        [Route("orders")]
        public ActionResult<IEnumerable<OrderTO>> GetAllOrders()
        {
            return _enterpriseService.GetAllOrders().ToArray();
        }

        /// <summary>
        /// method <c>GetAllStores</c> is an http get endpoint to request all
        /// store entries from the database
        /// </summary>
        /// <returns>
        /// list of store entires
        /// </returns>
        [HttpGet]
        [Route("stores")]
        public ActionResult<IEnumerable<Store>> GetAllStores()
        {
            return _enterpriseService.GetAllStores().ToArray();
        }

        /// <summary>
        /// method <c>GetAllProviders</c> is an http get endpoint to request all
        /// provider entries from the database
        /// </summary>
        /// <returns>
        /// list of all provider entries
        /// </returns>
        [HttpGet]
        [Route("provider")]
        public ActionResult<IEnumerable<Provider>> GetAllProvider()
        {
            return _enterpriseService.GetAllProvider().ToArray();
        }

        /// <summary>
        /// method <c>GetAllProducts</c> is an http get endpoint to request all
        /// product entries from the database
        /// </summary>
        /// <returns>
        /// list of all product entries
        /// </returns>
        [HttpGet]
        [Route("products")]
        public ActionResult<IEnumerable<ProductTO>> GetAllProducts()
        {
            return _enterpriseService.GetAllProducts().ToArray();
        }

        /// <summary>
        /// method <c>GetStores</c> is an http get endpoint to request all stores
        /// which have the product with the given id in their stockitems
        /// </summary>
        /// <param name="id">unique identifier of the produxt</param>
        /// <returns>
        /// list of store entries
        /// </returns>
        [HttpGet]
        [Route("product/{id}/stores")]
        public ActionResult<IEnumerable<Store>> GetStores(int id)
        {
            return _enterpriseService.GetStores(id).ToArray();
        }

        /// <summary>
        /// method <c>CreateProduct</c> is an http post endpoint to create a new
        /// product entry in the database
        /// </summary>
        /// <param name="product">
        /// tranfer object containing the new products data
        /// </param>
        /// <returns>new product entry as transfer object</returns>
        [HttpPost]
        [Route("create-product")]
        public ActionResult<ProductTO> CreateProduct(ProductTO product)
        {
            try
            {
                return _enterpriseService.CreateProduct(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// method <c>UpdateProduct</c> is an http put endpoint to modify
        /// a product entry in the database
        /// </summary>
        /// <param name="id">unique identifier of the product</param>
        /// <param name="product">transfer object containing the new data</param>
        /// <returns>
        /// updated product entry or status not found if product was not found
        /// or status bad request if another error occurs
        /// </returns>
        [HttpPut]
        [Route("update-product/{id}")]
        public ActionResult<ProductTO> UpdateProduct(int id, ProductTO product)
        {
            try
            {
                return _enterpriseService.UpdateProduct(id, product);
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
        /// method <c>CreateStore</c> is an http post endpoint to create a new
        /// store enty in the database
        /// </summary>
        /// <param name="store">object containing the new data</param>
        /// <returns>updated store entry or bad request in case of error</returns>
        [HttpPost]
        [Route("create-store")]
        public ActionResult<Store> CreateStore(Store store)
        {
            try
            {
                return _enterpriseService.CreateStore(store);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// method <c>UpdateStore</c> is an http put endpoint to modify an store
        /// entry in database
        /// </summary>
        /// <param name="id">unique identifier of the store</param>
        /// <param name="store">object containing the new data</param>
        /// <returns>
        /// updated store object or status bad request if store was not found
        /// or status bad request in case of other errors
        /// </returns>
        [HttpPut]
        [Route("update-store/{id}")]
        public ActionResult<Store> UpdateStore(int id, Store store)
        {
            try
            {
                return _enterpriseService.UpdateStore(id, store);
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
        /// method <c>CreateProvider</c> is an http post endpoint to create a new
        /// provider entry in the database
        /// </summary>
        /// <param name="provider">object containing the new data</param>
        /// <returns>
        /// new provider entry or status bad request in case of errors
        /// </returns>
        [HttpPost]
        [Route("create-provider")]
        public ActionResult<Provider> CreateProvider(Provider provider)
        {
            try
            {
                return _enterpriseService.CreateProvider(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// method <c>UpdateProvider</c> is an http put endpoint to modify an
        /// provider entry in database
        /// </summary>
        /// <param name="id">unique identifier of the provider</param>
        /// <param name="provider">object containing the new data</param>
        /// <returns>
        /// updated provider object or status bad request if provider was not found
        /// or status bad request in case of other errors
        /// </returns>
        [HttpPut]
        [Route("update-provider/{id}")]
        public ActionResult<Provider> UpdateProvider(int id, Provider provider)
        {
            try
            {
                return _enterpriseService.UpdateProvider(id, provider);
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
        /// method <c>CreateStock</c> is an http post method to create a new
        /// stock item entry with the given product for the store with the given id
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="product">
        /// data transfer object containing information of the product that should
        /// be added to stock
        /// </param>
        /// <returns>updated list of store entries that have the product in stock</returns>
        [HttpPost]
        [Route("create-stock/{storeId}")]
        public ActionResult<IEnumerable<Store>> CreateStock(int storeId, ProductTO product)
        {
            try
            {
                _logger.LogInformation(
                    "adding product {} to store {}", product.Id, storeId);
                _enterpriseService.AddToStock(storeId, product.Id);
                return _enterpriseService.GetStores(product.Id).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// method <c>GetStoreReports</c> is an http get endpoint to request
        /// a report of all stores current years profits
        /// </summary>
        /// <returns>list of profit reports labeled with store name</returns>
        [HttpGet]
        [Route("store-reports")]
        public IEnumerable<Report> GetStoreReports()
        {
            return _reportService.GetLatestProfit().ToArray();
        }

        /// <summary>
        /// method <c>GetDeliveryReports</c> is a http get endpoint to
        /// request all providers delivery reports
        /// </summary>
        /// <returns>
        /// array containing the delivery report of each provider in database
        /// </returns>
        [HttpGet]
        [Route("provider-reports")]
        public IEnumerable<Report> GetDeliveryReports()
        {
            return _reportService.GetGeneralDeliveryReports().ToArray();
        }
    }
}
