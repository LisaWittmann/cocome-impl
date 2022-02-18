using System;
using System.Collections.Generic;
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
        private bool inExpressMode = false;
        private Dictionary<Sale, int> lastSales = new Dictionary<Sale, int>();

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
        public ActionResult<bool> ConfirmCheckout(int id, IEnumerable<SaleElementTO> elements)
        {
            try
            {
                _logger.LogInformation("confirm checkout");
                Sale currentSale = _service.CreateSale(id, elements);
                int numberOfElements = 0;
                foreach (SaleElementTO element in elements)
                {
                    numberOfElements++;
                }
                UpdateExpressMode(currentSale, numberOfElements);
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

        private void UpdateExpressMode(Sale sale, int numberOfSaleItems)
        {
            lastSales.Add(sale, numberOfSaleItems);
            DateTime lastHour = DateTime.Now.AddHours(-1);
            int validSales = 0;
            foreach (KeyValuePair<Sale, int> saleEntry in lastSales)
            {
                if(saleEntry.Key.TimeStamp < lastHour)
                {
                    lastSales.Remove(saleEntry.Key);
                } else if (saleEntry.Value < 9)
                {
                    validSales++;
                }
            }

            if (validSales * 2 >= lastSales.Count) { inExpressMode = true; }

        }

    }
}
