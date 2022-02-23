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
using Microsoft.AspNetCore.Http;

namespace CocomeStore.Controllers
{
    /// <summary>
    /// class <c>EnterpriseController</c> provides REST endpoints for the
    /// enterprise application and requires authorization of enterprise policy
    /// </summary>
    [ApiController]
    [Authorize(Policy = "enterprise")]
    [Route("api/[controller]")]
    [Produces("application/json")]
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
        /// endpoint to request all store entries from the database
        /// </summary>
        /// <returns>
        /// list of store enties
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/enterprise/stores
        ///   
        /// </remarks>
        /// <response code="200">list of all store entities</response>
        [HttpGet]
        [Route("stores")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Store>> GetAllStores()
        {
            return _enterpriseService.GetAllStores().ToArray();
        }

        /// <summary>
        /// endpoint to create a new store enty in the database
        /// </summary>
        /// <param name="store">object containing the new data</param>
        /// <returns>updated store entry or bad request in case of error</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/enterprise/stores
        ///     {
        ///         "name": "Filiale Test",
        ///         "city": "Wiesbaden",
        ///         "postalCode": 65183,
        ///     }
        ///   
        /// </remarks>
        /// <response code="200">new store was added to database</response>
        /// <response code="400">store could not be created</response>
        [HttpPost]
        [Route("stores")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// endpoint to modify an store entry in database
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="store">object containing the new data</param>
        /// <returns>
        /// updated store object or status bad request if store was not found
        /// or status bad request in case of other errors
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/enterprise/stores/1
        ///     {
        ///         "name": "Filiale Test",
        ///         "city": "Wiesbaden",
        ///         "postalCode": 65183,
        ///     }
        ///   
        /// </remarks>
        /// <response code="200">store was successfully updated</response>
        /// <response code="404">store to modidy could not be found</response>
        /// <response code="400">store could not be updated</response>
        [HttpPut]
        [Route("stores/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Store> UpdateStore(int storeId, Store store)
        {
            try
            {
                return _enterpriseService.UpdateStore(storeId, store);
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
        /// endpoint to create a new stock item entry with the given product for
        /// the store with the given id
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="product">
        /// data transfer object containing information of the product that should
        /// be added to stock
        /// </param>
        /// <returns>updated list of store entries that have the product in stock</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/enterprise/stores/1/stock
        ///     {
        ///         "id": 1,
        ///         "name": "Eistee Zitrone",
        ///         "price": 0.69,
        ///         "salePrice": 1.39,
        ///         "description": "",
        ///         "imageUrl": "",
        ///         "provider": {
        ///             "id": 1,
        ///             "name": "Spedition Heinrich"
        ///         }
        ///     }
        ///   
        /// </remarks>
        /// <response code="200">product was added to stores stock</response>
        /// <response code="404">product or store was not found</response>
        /// <response code="400">product could not be added to stock</response>
        [HttpPost]
        [Route("stores/{storeId}/stock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// endpoint to request all product entries from the database
        /// </summary>
        /// <returns>
        /// list of all product entries
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/enterprise/products
        ///   
        /// </remarks>
        /// <response code="200">all product entries</response>
        [HttpGet]
        [Route("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductTO>> GetAllProducts()
        {
            return _enterpriseService.GetAllProducts().ToArray();
        }

        /// <summary>
        /// endpoint to create a new product entry in the database
        /// </summary>
        /// <param name="product">
        /// tranfer object containing the new products data
        /// </param>
        /// <returns>new product entry as transfer object</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///      POST /api/enterprise/products
        ///      {
        ///         "id": 0,
        ///         "name": "Testprodukt",
        ///         "price": 0.69,
        ///         "salePrice": 1.39,
        ///         "description": "",
        ///         "imageUrl": "",
        ///         "provider": {
        ///             "id": 1,
        ///             "name": "Spedition Heinrich"
        ///         }
        ///      }
        ///   
        /// </remarks>
        /// <response code="200">product was successfully added to database</response>
        /// <response code="400">product could not be created</response>
        [HttpPost]
        [Route("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// endpoint to modify a product entry in the database
        /// </summary>
        /// <param name="productId">unique identifier of the product</param>
        /// <param name="product">transfer object containing the new data</param>
        /// <returns>
        /// updated product entry or status not found if product was not found
        /// or status bad request if another error occurs
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///      PUT /api/enterprise/products/1
        ///      {
        ///         "id": 1,
        ///         "name": "Neuer Produktname",
        ///         "price": 0.69,
        ///         "salePrice": 1.39,
        ///         "description": "",
        ///         "imageUrl": "",
        ///         "provider": {
        ///             "id": 1,
        ///             "name": "Spedition Heinrich"
        ///         }
        ///      }
        ///   
        /// </remarks>
        /// <response code="200">product was successfully updated</response>
        /// <response code="404">product with id was not found</response>
        /// <response code="400">product could not be updated</response>
        [HttpPut]
        [Route("products/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ProductTO> UpdateProduct(int productId, ProductTO product)
        {
            try
            {
                return _enterpriseService.UpdateProduct(productId, product);
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
        /// endpoint to request all stores which have the product with the given
        /// id in their stockitems
        /// </summary>
        /// <param name="productId">unique identifier of the product</param>
        /// <returns>
        /// list of store entries
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///      GET /api/enterprise/1/stores
        ///   
        /// </remarks>
        /// <response code="200">returns all stores which have the product in stock</response>
        [HttpGet]
        [Route("products/{productId}/stores")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Store>> GetStores(int productId)
        {
            return _enterpriseService.GetStores(productId).ToArray();
        }

        /// <summary>
        /// endpoint to request all provider entries from the database
        /// </summary>
        /// <returns>
        /// list of all provider entries
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///      GET /api/enterprise/providers
        ///   
        /// </remarks>
        /// <response code="200">returns all provider entries</response>
        [HttpGet]
        [Route("providers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Provider>> GetAllProvider()
        {
            return _enterpriseService.GetAllProvider().ToArray();
        }

        /// <summary>
        /// endpoint to create a new provider entry in the database
        /// </summary>
        /// <param name="provider">object containing the new data</param>
        /// <returns>
        /// new provider entry or status bad request in case of errors
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///      POST /api/enterprise/providers/1
        ///      {
        ///         "id": 0,
        ///         "name": "Lieferant Test",
        ///      }
        ///   
        /// </remarks>
        /// <response code="200">provider was successfully added to database</response>
        /// <response code="400">provider could not be created</response>
        [HttpPost]
        [Route("providers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// endpoint to modify a provider entry in database
        /// </summary>
        /// <param name="providerId">unique identifier of the provider</param>
        /// <param name="provider">object containing the new data</param>
        /// <returns>
        /// updated provider object or status bad request if provider was not found
        /// or status bad request in case of other errors
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///      POST /api/enterprise/stores/1/stock
        ///      {
        ///         "id": 1,
        ///         "name": "Neuer Lieferanten Name",
        ///      }
        ///   
        /// </remarks>
        /// <response code="200">provider was successfully updated</response>
        /// <response code="404">provider with id was not found</response>
        /// <response code="400">provider could not be updated</response>
        [HttpPut]
        [Route("providers/{providerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Provider> UpdateProvider(int providerId, Provider provider)
        {
            try
            {
                return _enterpriseService.UpdateProvider(providerId, provider);
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
        /// endpoint to request all providers delivery reports
        /// </summary>
        /// <returns>
        /// array containing the delivery report of each provider in database
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///      GET /api/enterpise/reports/delivery
        ///   
        /// </remarks>
        /// <response code="200">list of delivery reports of each provider</response>
        [HttpGet]
        [Route("reports/delivery")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Report> GetDeliveryReports()
        {
            return _reportService.GetGeneralDeliveryReports().ToArray();
        }

        /// <summary>
        /// endpoint to request a report of all stores current years profits
        /// </summary>
        /// <returns>list of profit reports labeled with store name</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///      GET /api/enterpise/reports/profits
        ///   
        /// </remarks>
        /// <response code="200">list of profit reports of each store</response>
        [HttpGet]
        [Route("reports/profits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Report> GetStoreReports()
        {
            return _reportService.GetLatestProfit().ToArray();
        }
    }
}
