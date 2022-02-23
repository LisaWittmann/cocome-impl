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
using Microsoft.AspNetCore.Http;

namespace CocomeStore.Controllers
{
    /// <summary>
    /// class <c>StoreController</c> provides REST endpoints for the store
    /// application and requires authorization of store policy
    /// </summary>
    [ApiController]
    [Authorize(Policy = "store")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;
        private readonly IStoreService _storeService;
        private readonly IExchangeService _exchangeService;
        private readonly IReportService _reportService;

        public StoreController(
            ILogger<StoreController> logger,
            IStoreService storeService,
            IExchangeService exchangeService,
            IReportService reportService
        )
        {
            _logger = logger;
            _storeService = storeService;
            _reportService = reportService;
            _exchangeService = exchangeService;
        }

        /// <summary>
        /// endpoint to request a store database entry by its id
        /// </summary>
        /// <param name="storeId">unique identitifier of the requested store</param>
        /// <returns>store object as response or status not found</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/store/1
        ///   
        /// </remarks>
        /// <response code="200">returns store entry with given order</response>
        /// <response code="404">store was not found</response>
        [HttpGet]
        [Route("{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Store> GetStore(int storeId)
        {
            try
            {
                _logger.LogInformation("requesting store by id {}", storeId);
                return _storeService.GetStore(storeId);
            }
            catch(EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// endpoint to request a stores overall profit by storeid
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <returns>
        /// profit report or not found if the store entry was not found
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/store/profit/1
        ///   
        /// </remarks>
        /// <response code="200">overall reports of the stores profit</response>
        /// <response code="404">store was not found</response>
        [HttpGet]
        [Route("profit/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Report>> GetProfit(int storeId)
        {
            try
            {
                return _reportService.GetStoreProfit(storeId).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// endpint to request a stores profit by year
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="year">year to filter profit for</param>
        /// <returns>
        /// report object containing the years monthly profits
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/store/profit/1/2022
        ///   
        /// </remarks>
        /// <response code="200">report of the stores profit in the requested year</response>
        /// <response code="404">store was not found</response>
        [HttpGet]
        [Route("profit/{storeId}/{year}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Report> GetProfit(int storeId, int year)
        {
            try
            {
                return _reportService.GetProfitOfYear(storeId, year);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// endpoint to request a stores stockitems by storeid
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <returns>list of stockitems that are related with the store</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/store/inventory/1
        ///   
        /// </remarks>
        /// <response code="200">all stock items related with the store</response>
        [HttpGet]
        [Route("inventory/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StockItem>> GetInventory(int storeId)
        {
            _logger.LogInformation("requesting inventory of store {}", storeId);
            return _storeService.GetInventory(storeId).ToArray();
            
        }


        /// <summary>
        /// endpoint to request a stores latest orders by storeid
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <returns>
        /// list of orders as transfer objects that are related with the store
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/store/orders/1
        ///   
        /// </remarks>
        /// <response code="200">latest orders related to the store</response>
        [HttpGet]
        [Route("orders/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrderTO>> GetOrders(int storeId)
        {
            _logger.LogInformation("requesting orders of store {}", storeId);
            return _storeService.GetOrders(storeId).ToArray();
        }

        /// <summary>
        /// endpoint to create a new order for the store with the given id and
        /// the incomming order elements as transfer objects
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="elements">transfer objects of order elements</param>
        /// <returns>
        /// list of orders as transfer objects that are related with the store
        /// to update the dataset of the requested system with the new database
        /// entries
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/store/orders/1
        ///     [{
        ///         "amount": 10,
        ///         "product": {
        ///             "id": 1,
        ///             "name": "Eistee Zitrone",
        ///             "price": 0.69,
        ///             "salePrice": 1.39,
        ///             "description": "",
        ///             "imageUrl: "",
        ///             "provider": {
        ///                 "id": 1,
        ///                 "name": "Spedition Heinrich",
        ///             }
        ///          }
        ///     }]
        ///   
        /// </remarks>
        /// <response code="200">order was successfully placed</response>
        /// <response code="400">order could not be placed</response>
        [HttpPost]
        [Route("orders/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<OrderTO>> PlaceOrder(int storeId, IEnumerable<OrderElementTO> elements)
        {
            try
            {
                _logger.LogInformation("place new order for store {}", storeId);
                _storeService.PlaceOrder(storeId, elements);
                return _storeService.GetOrders(storeId).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// endpoint to mark an order as delivered and add the order element to
        /// the stores stock items
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="orderTO">
        /// order transfer object containing the id of the object to change
        /// </param>
        /// <returns>updates list of orders</returns>
        /// <response code="200">order was closed and items were added to stock</response>
        /// <response code="400">order could not be closed</response>
        /// <response code="404">order was not found</response>
        [HttpPost]
        [Route("orders/{storeId}/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<OrderTO>> CloseOrder(int storeId, OrderTO orderTO)
        {
            try
            {
                _logger.LogInformation("close order with id {} for store {}", orderTO.Id, storeId);
                _storeService.CloseOrder(storeId, orderTO.Id);
                return _storeService.GetOrders(storeId).ToArray();
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
        /// endpoint to modify a product of the inventory of the store with the given id
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="productTO">
        /// product transfer object containing the updated product data
        /// </param>
        /// <returns>
        /// list of all stockitems of the store to update requesting systems data
        /// after database changes or status conflict if the product is not in the
        /// inventory of the store or status bad request if an intern error accurs
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/store/products/1
        ///     {
        ///         "id": 1,
        ///         "name": "Neuer Name",
        ///         "price": 0.69,
        ///         "salePrice": 2.51,
        ///         "description": "Neue Beschreibung",
        ///         "imageUrl": "Image/url.png",
        ///         "provider": {
        ///             "id": 1,
        ///             "name": "Spedition Heinrich",
        ///          }
        ///     }
        ///   
        /// </remarks>
        /// <response code="200">product was successfully updated</response>
        /// <response code="409">product is not in stock of the reuesting store</response>
        /// <response code="400">product could not be updated</response>
        [HttpPut]
        [Route("products/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<StockItem>> UpdateProduct(int storeId, ProductTO productTO)
        {
            try
            {
                _logger.LogInformation("updating product {} from store {}", productTO.Name, storeId);
                _storeService.UpdateProduct(storeId, productTO);
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
            return _storeService.GetInventory(storeId).ToArray();
        }

        /// <summary>
        /// endpint to request a product of the stores inventory by its id
        /// </summary>
        /// <param name="storeId">unique identitfier of the store</param>
        /// <param name="productId">unique identitfier of the product</param>
        /// <returns>
        /// the requested product as transfer object or status conflict if the
        /// product is not in the stockitems of the store
        /// </returns>
        [HttpGet]
        [Route("products/{storeId}/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ProductTO> GetProduct(int storeId, int productId)
        {
            try
            {
                _logger.LogInformation("requesting product {} of store {}", productId, storeId);
                return _storeService.GetProduct(storeId, productId);
            }
            catch (CrossAccessException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict();
            }
        }


        /// <summary>
        /// endpoint to request a stores outstanding stock exchanges with another store
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <returns>list of stock exchange entries as trasnfer object</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/store/exchanges/1
        ///   
        /// </remarks>
        /// <response code="200">return list of latest store exchanges</response>
        [HttpGet]
        [Route("exchanges/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StockExchangeTO>> GetExchanges(int storeId)
        {
            return _exchangeService.GetStockExchanges(storeId).ToArray();
        }


        /// <summary>
        /// endpoint to initiate a stock exchange and therefore update the placing
        /// date and remove the products from the providing stores inventory
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="exchangeTO">information about the exchange to update</param>
        /// <returns>list of updated latest store exchanges</returns>
        /// <response code="200">exchanges was started and items were removed from stock</response>
        /// <response code="404">exchange was not found</response>
        [HttpPut]
        [Route("exchanges/{storeId}/start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StockExchangeTO>> StartExchange(int storeId, StockExchangeTO exchangeTO)
        {
            try
            {
                _exchangeService.StartStockExchange(exchangeTO);
                return _exchangeService.GetStockExchanges(storeId).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
            
        }

        /// <summary>
        /// endpoint to initiate a stock exchange and therefore update the
        /// delivering date and add the products to the receiving stores inventory
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="exchangeTO">information about the exchange to update</param>
        /// <returns>list of updated latest store exchanges</returns>
        [HttpPut]
        [Route("exchanges/{storeId}/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StockExchangeTO>> CloseExchange(int storeId, StockExchangeTO exchangeTO)
        {
            try
            {
                _exchangeService.CloseStockExchange(exchangeTO);
                return _exchangeService.GetStockExchanges(storeId).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }

        }
    }
}
