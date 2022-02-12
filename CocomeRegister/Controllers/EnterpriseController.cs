﻿using CocomeStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CocomeStore.Services;
using System;
using CocomeStore.Exceptions;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseController : ControllerBase
    {
        private readonly ILogger<EnterpriseController> _logger;
        private readonly IEnterpriseService _service;

        public EnterpriseController(
            ILogger<EnterpriseController> logger,
            IEnterpriseService service
        )
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("orders")]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            return _service.GetAllOrders().ToArray();
        }

        [HttpGet]
        [Route("inventories")]
        public ActionResult<IEnumerable<StockItem>> GetAllStockItems()
        {
            return _service.GetAllStockItems().ToArray();
        }

        [HttpGet]
        [Route("stores")]
        public ActionResult<IEnumerable<Store>> GetAllStores()
        {
            return _service.GetAllStores().ToArray();
        }

        [HttpGet]
        [Route("provider")]
        public ActionResult<IEnumerable<Provider>> GetAllProvider()
        {
            return _service.GetAllProvider().ToArray();
        }

        [HttpGet]
        [Route("products")]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            return _service.GetAllProducts().ToArray();
        }

        [HttpPost]
        [Route("create-product")]
        public ActionResult<IEnumerable<Product>> CreateProduct(Product product)
        {
            try
            {
                _service.CreateProduct(product);
                return _service.GetAllProducts().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update-product/{id}")]
        public ActionResult<IEnumerable<Product>> UpdateProduct(int id, Product product)
        {
            try
            {
                _service.UpdateProduct(id, product);
                return _service.GetAllProducts().ToArray();
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

        [HttpPost]
        [Route("create-store")]
        public ActionResult<IEnumerable<Store>> CreateStore(Store store)
        {
            try
            {
                _service.CreateStore(store);
                return _service.GetAllStores().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update-store/{id}")]
        public ActionResult<IEnumerable<Store>> UpdateStore(int id, Store store)
        {
            try
            {
                _service.UpdateStore(id, store);
                return _service.GetAllStores().ToArray();
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

        [HttpPost]
        [Route("create-provider")]
        public ActionResult<IEnumerable<Provider>> CreateProvider(Provider provider)
        {
            try
            {
                _service.CreateProvider(provider);
                return _service.GetAllProvider().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("update-provider/{id}")]
        public ActionResult<IEnumerable<Provider>> UpdateProvider(int id, Provider provider)
        {
            try
            {
                _service.UpdateProvider(id, provider);
                return _service.GetAllProvider().ToArray();
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
