using System;
using System.Collections.Generic;
using System.Linq;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;
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

        [HttpGet]
        [Route("products/{storeId}")]
        public ActionResult<IEnumerable<Product>> GetProducts(int storeId)
        {
            return _service.GetAvailableProducts(storeId).ToArray();
        }

        [HttpPost]
        [Route("update-express/{id}")]
        public bool EndExpressMode(int id, bool expressMode)
        {
            _logger.LogInformation("updating expressMode of cashdesk {} to {}", id, expressMode);
            inExpressMode = expressMode;
            return inExpressMode;
        }

        [HttpPost]
        [Route("checkout/{storeId}")]
        public ActionResult<IEnumerable<Product>> ConfirmCheckout(int storeId, IEnumerable<SaleElementTO> elements)
        {
            try
            {
                _logger.LogInformation("confirm checkout");
                _service.CreateSale(storeId, elements);
                return _service.GetAvailableProducts(storeId).ToArray();
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
