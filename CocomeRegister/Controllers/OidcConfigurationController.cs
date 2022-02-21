using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace CocomeStore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class OidcConfigurationController : Controller
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientRequestParametersProvider"></param>
        /// <param name="logger"></param>
        public OidcConfigurationController(
            IClientRequestParametersProvider clientRequestParametersProvider
        )
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("_configuration/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute]string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }
    }
}
