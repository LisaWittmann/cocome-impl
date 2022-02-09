using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseController : ControllerBase
    {
        private readonly ILogger<EnterpriseController> _logger;

        public EnterpriseController(ILogger<EnterpriseController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("test")]
        public int GetTestNumber()
        {
            return 12;
        }
    }
}
