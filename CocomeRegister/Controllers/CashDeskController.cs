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
    [ApiController]
    [Authorize(Policy = "cashdesk")]
    [Route("api/[controller]")]
    public class CashDeskController : Controller
    {
        private readonly ILogger<CashDeskController> _logger;
        private readonly IUriService _uriService;
        private readonly IDocumentService _documentService;
        private readonly ICashDeskService _service;

        // testData
        private bool inExpressMode = true;

        public CashDeskController(
            ILogger<CashDeskController> logger,
            IUriService uriService,
            ICashDeskService service,
            IDocumentService documentService
        )
        {
            _logger = logger;
            _service = service;
            _uriService = uriService;
            _documentService = documentService;
        }

        [HttpGet]
        [Route("express/{id}")]
        public bool GetExpressMode(int id)
        {
            _logger.LogInformation("requesting expressMode of cashdesk {}", id);
            return inExpressMode;
        }

        [HttpPost]
        [Route("update-express/{id}")]
        public bool EndExpressMode(int id, bool expressMode)
        {
            _logger.LogInformation("updating expressMode of cashdesk {} to {}", id, expressMode);
            inExpressMode = expressMode;
            return inExpressMode;
        }

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

        [HttpGet]
        [Route("products/{storeId}")]
        public ActionResult<PagedResponse<Product>> GetProducts(int storeId, [FromQuery] string q, [FromQuery] PaginationFilter filter)
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
            return responseBuilder.CreatePagedResponse(data, filter, _uriService, route);
        }

        [HttpPost]
        [Route("checkout/{storeId}")]
        public async Task<IActionResult> ConfirmCheckout(int storeId, SaleTO saleTO)
        {
            try
            {
                _logger.LogInformation("confirm checkout");
                _service.CreateSale(storeId, saleTO);
                var billing = await _documentService.CreateBill(_service.CreateSale(storeId, saleTO));
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

    }
}
