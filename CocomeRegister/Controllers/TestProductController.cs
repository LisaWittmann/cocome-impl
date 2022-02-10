﻿using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestProductController : ControllerBase
    {
        private CocomeDbContext _context;
        private readonly ILogger<TestProductController> _logger;

        public TestProductController(ILogger<TestProductController> logger, CocomeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public Product GetProduct(int id)
        {
            _logger.LogInformation("requesting product from database with id {}", id);
            Product result = _context.Products.Find(id);
            return result;
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products;
        }

        // GET api/Test/5
        /*[HttpGet]
        [Route("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "Your ID is " + id;
        }*/

        /*// GET: TestController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
