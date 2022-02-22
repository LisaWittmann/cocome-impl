﻿using System;
using System.Linq;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CocomeStore.Services;
using CocomeStore.Exceptions;
using CocomeStore.Models.Transfer;
using Microsoft.AspNetCore.Authorization;

namespace CocomeStore.Controllers
{
    // <summary>
    /// class <c>StoreController</c> provides REST endpoints for the store
    /// application and requires authorization of store policy
    /// </summary>
    [ApiController]
    [Authorize(Policy = "store")]
    [Route("api/[controller]")]
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;
        private readonly IStoreService _service;
        private readonly IDatabaseStatistics _statistics;

        public StoreController(
            ILogger<StoreController> logger,
            IStoreService service,
            IDatabaseStatistics statistics
        )
        {
            _service = service;
            _logger = logger;
            _statistics = statistics;
        }

        /// <summary>
        /// method <c>GetStore</c> is an http get endpoint to request a store
        /// database entry by its id
        /// </summary>
        /// <param name="id">unique identitifier of the requested store</param>
        /// <returns>store object as response or status not found</returns>
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Store> GetStore(int id)
        {
            try
            {
                _logger.LogInformation("requesting store by id {}", id);
                return _service.GetStore(id);
            }
            catch(EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// method <c>GetProfit</c> is an http get endpoint to request a stores
        /// overall profit by storeid
        /// </summary>
        /// <param name="id">unique identifier of the store</param>
        /// <returns>
        /// profit statistics or not found if the store entry was not found
        /// </returns>
        [HttpGet]
        [Route("profit/{id}")]
        public ActionResult<IEnumerable<Statistic>> GetProfit(int id)
        {
            try
            {
                return _statistics.GetStoreProfit(id).ToArray();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// method <c>GetInventory</c> is an http get endpoint to request a stores
        /// stockitems by storeid
        /// </summary>
        /// <param name="id">unique identitfier of the store</param>
        /// <returns>list of stockitems that are related with the store</returns>
        [HttpGet]
        [Route("inventory/{id}")]
        public ActionResult<IEnumerable<StockItem>> GetInventory(int id)
        {
            _logger.LogInformation("requesting inventory of store {}", id);
            return _service.GetInventory(id).ToArray();
            
        }

        /// <summary>
        /// method <c>GetOrders</c> is an http get endpoint to requets a stores
        /// overall orders by storeid
        /// </summary>
        /// <param name="id">unique identitfier of the store</param>
        /// <returns>
        /// list of orders as transfer objects that are related with the store
        /// </returns>
        [HttpGet]
        [Route("orders/{id}")]
        public ActionResult<IEnumerable<OrderTO>> GetOrders(int id)
        {
            _logger.LogInformation("requesting orders of store {}", id);
            return _service.GetOrders(id).ToArray();
        }

        /// <summary>
        /// method <c>PlaceOrder</c> is an http get endpoint to create a new order
        /// for the store with the given id and the incomming order elements as
        /// transfer objects
        /// </summary>
        /// <param name="id">unique identitfier of the store</param>
        /// <param name="elements">transfer objects of order elements</param>
        /// <returns>
        /// list of orders as transfer objects that are related with the store
        /// to update the dataset of the requested system with the new database
        /// entries
        /// </returns>
        [HttpPost]
        [Route("create-order/{id}")]
        public ActionResult<IEnumerable<OrderTO>> PlaceOrder(int id, IEnumerable<OrderElementTO> elements)
        {
            try
            {
                _logger.LogInformation("place new order for store {}", id);
                _service.PlaceOrder(id, elements);
                return _service.GetOrders(id).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// method <c>CloseOrder</c> is an http post endpoint to mark an order as
        /// delivered
        /// </summary>
        /// <param name="id">unique identitfier of the store</param>
        /// <param name="orderTO">
        /// order transfer object containing the id of the object to change
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("close-order/{id}")]
        public ActionResult<IEnumerable<OrderTO>> CloseOrder(int id, OrderTO orderTO)
        {
            try
            {
                _logger.LogInformation("close order with id {} for store {}", orderTO.Id, id);
                _service.CloseOrder(id, orderTO.Id);
                return _service.GetOrders(id).ToArray();
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

        /// <summary>
        /// method <c>UpdateProduct</c> is an http post endpoint to modify a product
        /// of the inventory of the store with the given id
        /// </summary>
        /// <param name="id">unique identitfier of the store</param>
        /// <param name="productTO">
        /// product transfer object containing the updated product data
        /// </param>
        /// <returns>
        /// list of all stockitems of the store to update requesting systems data
        /// after database changes or status conflict if the product is not in the
        /// inventory of the store or status bad request if an intern error accurs
        /// </returns>
        [HttpPost]
        [Route("update-product/{id}")]
        public ActionResult<IEnumerable<StockItem>> UpdateProduct(int id, ProductTO productTO)
        {
            try
            {
                _logger.LogInformation("updating product {} from store {}", productTO.Name, id);
                _service.UpdateProduct(id, productTO);
            }
            catch (CrossAccessException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
            return _service.GetInventory(id).ToArray();
        }

        /// <summary>
        /// method <c>GetProduct</c> is an http get endpint to request a product
        /// of the stores inventory by its id
        /// </summary>
        /// <param name="id">unique identitfier of the store</param>
        /// <param name="productId">unique identitfier of the product</param>
        /// <returns>
        /// the requested product as transfer object or status conflict if the
        /// product is not in the stockitems of the store
        /// </returns>
        [HttpGet]
        [Route("{id}/product/{productId}")]
        public ActionResult<ProductTO> GetProduct(int id, int productId)
        {
            try
            {
                _logger.LogInformation("requesting product {} of store {}", productId, id);
                return _service.GetProduct(id, productId);
            }
            catch (CrossAccessException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict();
            }
        }

        /// <summary>
        /// method <c>GetProfit</c> is an http get endpint to request a stores
        /// profit by year
        /// </summary>
        /// <param name="id">unique identitfier of the store</param>
        /// <param name="year">year to filter profit for</param>
        /// <returns>
        /// statistic object containing the years monthly profits
        /// </returns>
        [HttpGet]
        [Route("{id}/profit/{year}")]
        public ActionResult<Statistic> GetProfit(int id, int year)
        {
            try
            {
                return _statistics.GetProfitOfYear(id, year);
            }
            catch(EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

    }
}
