using System;
using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CashDeskController : ControllerBase
    {
        private readonly ILogger<CashDeskController> _logger;
        private readonly ICashDeskService _cashDeskService;

        public CashDeskController(ILogger<CashDeskController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Vorrat> GetInventory()
        {
            _logger.LogInformation("requesting inventory");
            return null;
        }

        [HttpPost]
        public void ConfirmCheckout()
        {
            _logger.LogInformation("confirm checkout");
        }
    }
}
