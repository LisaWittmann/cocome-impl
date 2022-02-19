using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;
using CocomeStore.Models.Transfer.Pagination;
using CocomeStore.Services;
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
        private readonly ICashDeskService _service;

        // testData
        private bool inExpressMode = true;

        public CashDeskController(
            ILogger<CashDeskController> logger,
            ICashDeskService service
        )
        {
            _logger = logger;
            _service = service;
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
        [Route("products/{storeId}")]
        public ActionResult<PagedResponse<Product>> GetProducts(int storeId, [FromQuery] PaginationFilter filter)
        {
            var responseBuilder = new ResponseBuilder<Product>();
            return responseBuilder.PaginateData(_service.GetAvailableProducts(storeId), filter);
        }

        [HttpPost]
        [Route("checkout/{storeId}")]
        public ActionResult<bool> ConfirmCheckout(int storeId, SaleTO saleTO)
        {
            try
            {
                _logger.LogInformation("confirm checkout");
                _service.CreateSale(storeId, saleTO);
                return true;
                
            }
            catch (ItemNotAvailableException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

    }
}
