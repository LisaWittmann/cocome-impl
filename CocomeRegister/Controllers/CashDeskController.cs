using System;
using System.Linq;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;
using CocomeStore.Models.Transfer.Pagination;
using CocomeStore.Services;
using CocomeStore.Services.Pagination;
using CocomeStore.Services.Documents;
using Microsoft.AspNetCore.Authorization;
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
    public class CashDeskController : Controller
    {
        private readonly ILogger<CashDeskController> _logger;
        private readonly IUriService _uriService;
        private readonly IDocumentService _documentService;
        private readonly IExchangeService _exchangeService;
        private readonly ICashDeskService _service;

        // TEST DATA
        // IN PROGRESS
        private bool inExpressMode = true;

        public CashDeskController(
            ILogger<CashDeskController> logger,
            IUriService uriService,
            ICashDeskService service,
            IExchangeService exchangeService,
            IDocumentService documentService
        )
        {
            _logger = logger;
            _service = service;
            _uriService = uriService;
            _exchangeService = exchangeService;
            _documentService = documentService;
        }

        // IN PROGRESS
        [HttpGet]
        [Route("express/{id}")]
        public bool GetExpressMode(int id)
        {
            _logger.LogInformation("requesting expressMode of cashdesk {}", id);
            return inExpressMode;
        }

        // IN PROGRESS
        [HttpPost]
        [Route("update-express/{id}")]
        public bool EndExpressMode(int id, bool expressMode)
        {
            _logger.LogInformation(
                "updating expressMode of cashdesk {} to {}",
                id,expressMode);
            inExpressMode = expressMode;
            return inExpressMode;
        }

        /// <summary>
        /// method <c>GetProduct</c> is an http get endpoint to request
        /// a product in a stores stock by its id
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="productId">unique identifier of the product</param>
        /// <returns></returns>
        [HttpGet]
        [Route("products/{storeId}/{productId}")]
        public ActionResult<Product> GetProduct(int storeId, int productId)
        {
            var product = _service.GetAvailableProducts(storeId)
                .Where(product => product.Id == productId)
                .SingleOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        /// <summary>
        /// method <c>GetProducts</c> is an http get endpoint to request the
        /// available porducts in a store
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
        [HttpGet]
        [Route("products/{storeId}")]
        public ActionResult<PagedResponse<Product>> GetProducts(
            int storeId,
            [FromQuery] string q,
            [FromQuery] PaginationFilter filter
        )
        {
            var route = Request.Path.Value;
            var responseBuilder = new ResponseBuilder<Product>();
            var data = _service.GetAvailableProducts(storeId);
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
        /// method <c>ConfirmCheckout</c> is an http post endpoint to confirm
        /// a sale on the cashdesk, update the stores stock and print the sale
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
        [HttpPost]
        [Route("checkout/{storeId}")]
        public async Task<IActionResult> ConfirmCheckout(
            int storeId, SaleTO saleTO)
        {
            try
            {
                _logger.LogInformation("confirm checkout");
                var billing = await _documentService.CreateBill
                    (await _service.CreateSale(storeId, saleTO)
                );
                _ = _exchangeService.CheckForExchanges(storeId);
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

    }
}
