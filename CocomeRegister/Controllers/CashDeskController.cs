using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashDeskController : ControllerBase
    {
        private readonly ILogger<CashDeskController> _logger;

        // testData
        private bool inExpressMode = true;

        public CashDeskController(ILogger<CashDeskController> logger)
        {
            _logger = logger;
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
        public void ConfirmCheckout(IEnumerable<Product> products)
        {
            _logger.LogInformation("confirm checkout");
        }

    }
}
