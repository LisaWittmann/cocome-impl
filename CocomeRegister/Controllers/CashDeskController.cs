using System.Linq;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;
using CocomeStore.Models.Transfer.Pagination;
using CocomeStore.Services;
using CocomeStore.Services.Bank;
using CocomeStore.Services.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Controllers
{
    /// <summary>
    /// class <c>CashDeskController</c> provided REST endpoints for the cashdesk
    /// in the store application and requires authorization of cashdesk policy
    /// </summary>
    [ApiController]
    [Authorize(Policy = "cashdesk")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CashDeskController : Controller
    {
        private readonly ILogger<CashDeskController> _logger;
        private readonly IUriService _uriService;
        private readonly IBankService _bankService;
        private readonly IPrinterService _printerService;
        private readonly IExchangeService _exchangeService;
        private readonly ICashDeskService _cashDeskService;

        public CashDeskController(
            ILogger<CashDeskController> logger,
            IUriService uriService,
            IBankService bankService,
            ICashDeskService service,
            IExchangeService exchangeService,
            IPrinterService printerService
        )
        {
            _logger = logger;
            _bankService = bankService;
            _cashDeskService = service;
            _uriService = uriService;
            _exchangeService = exchangeService;
            _printerService = printerService;
        }

        /// <summary>
        /// endpoint to request the available products in a store
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="q">search param to filter products by name or id</param>
        /// <param name="filter">
        /// pagefilter containing the requested page and the requested amount
        /// of elements per page
        /// </param>
        /// <returns>
        /// <see cref="PagedResponse{Product}"/> containing the product entries
        /// of the requested page as data
        /// </returns>
        /// <response code="200">products in requested format</response>
        [HttpGet]
        [Route("products/{storeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<PagedResponse<Product>> GetProducts(
            int storeId,
            [FromQuery] string q,
            [FromQuery] PaginationFilter filter
        )
        {
            var route = Request.Path.Value;
            var responseBuilder = new ResponseBuilder<Product>();
            var data = _cashDeskService.GetAvailableProducts(storeId);
            if (q != null)
            {
                data = data.Where(product =>
                    product.Id.ToString().Contains(q) ||
                    product.Name.ToLower().Contains(q.ToLower())
                );
            }
            return responseBuilder.CreatePagedResponse(
                data, filter, _uriService, route);
        }

        /// <summary>
        /// endpoint to request a product in a stores stock by its id
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="productId">unique identifier of the product</param>
        /// <returns>product with requested id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/cashdesk/products/1/1
        ///   
        /// </remarks>
        /// <response code="200">returns the requested product</response>
        /// <response code="404">product entry was not found</response>
        [HttpGet]
        [Route("products/{storeId}/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetProduct(int storeId, int productId)
        {
            var product = _cashDeskService.GetAvailableProducts(storeId)
                .Where(product => product.Id == productId)
                .SingleOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        /// <summary>
        /// endpoint to confirm a sale on the cashdesk, update the stores stock
        /// and print the sale
        /// billing
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="saleTO">
        /// transfer object containing the sale informations
        /// </param>
        /// <returns>
        /// the billing for the confirmed sale as pdf file for printing or status
        /// conflict if an element of the sale is out of stock or status not found
        /// if the store with the given id was not found or bad request if another
        /// error accurs
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/cashdesk/checkout/1
        ///     {
        ///         "saleElements": [],
        ///         "paymentMethod": 0,
        ///         "timeStamp": "\"2022-02-23T18:45:20.634Z\",
        ///         "total": 0,
        ///         "payed": 0
        ///     }
        ///   
        /// </remarks>
        /// <response code="200">returns billing as pdf for the sale</response>
        /// <response code="409">a product of the sale is out of stock</response>
        /// <response code="404">store was not found</response>
        [HttpPost]
        [Route("checkout/{storeId}")]
        [Produces("application/pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmCheckoutAsnyc(
            int storeId, SaleTO saleTO)
        {
            try
            {
                _logger.LogInformation("confirm checkout");
                saleTO = await _cashDeskService.UpdateSaleDataAsync(storeId, saleTO);
                var billing = await _printerService.CreateBillAsync(saleTO);
                await _cashDeskService.CreateSaleAsync(saleTO);
                await _exchangeService.CheckForExchangesAsync(storeId).ConfigureAwait(false);
                return File(billing, "application/pdf");
                
            }
            catch (ItemNotAvailableException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
            
        }

        /// <summary>
        /// endpoint to perform a card payment
        /// </summary>
        /// <param name="creditCardTO">transfer object containing the credit card information</param>
        /// <returns>status ok if payment was accepted or bad request if an error accurs</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/cashdesk/checkout/card
        ///     {
        ///         "number": "DE123455689"
        ///         "pin": 1234
        ///     }
        ///  
        /// </remarks>
        /// <response code="200">credit card payment confiremed</response>
        /// <response code="400">credit card was not accepted</response>
        [HttpPost]
        [Route("checkout/card")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmPaymentAsync(CreditCardTO creditCardTO)
        {
            try
            {
                await _bankService.ConfirmPaymentAsync(creditCardTO);
                return Ok();
            }
            catch (InvalidPaymentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
