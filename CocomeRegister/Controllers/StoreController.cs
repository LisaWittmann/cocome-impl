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
    public class StoreContoller : ControllerBase
    {
        private readonly ILogger<CashDeskController> _logger;
        private readonly IStoreService _storeService;

        public StoreContoller(ILogger<CashDeskController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Vorrat> GetInventory()
        {
            _logger.LogInformation("requesting inventory");
            return null;
        }
    }
}
