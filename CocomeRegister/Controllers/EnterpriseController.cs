using CocomeStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseController : ControllerBase
    {
        private readonly ILogger<EnterpriseController> _logger;
        private CocomeDbContext _context;

        public EnterpriseController(ILogger<EnterpriseController> logger, CocomeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("orders")]
        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                    .Include(order => order.Store)
                    .Include(order => order.Provider)
                    .ToArray();
        }

        [HttpGet]
        [Route("inventories")]
        public IEnumerable<StockItem> GetAllStockItems()
        {
            return _context.StockItems
                    .Include(item => item.Product)
                    .Include(item => item.Store)
                    .ToArray();
        }
    }
}
