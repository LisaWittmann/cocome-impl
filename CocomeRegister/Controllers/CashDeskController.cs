using System;
using System.Collections.Generic;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashDeskController : ControllerBase
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

        [HttpPost]
        [Route("checkout/{id}")]
        public ActionResult ConfirmCheckout(int id, IEnumerable<SaleElement> elements)
        {
            try
            {
                _logger.LogInformation("confirm checkout");
                _service.CreateSale(id, elements);
                return Ok();
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
