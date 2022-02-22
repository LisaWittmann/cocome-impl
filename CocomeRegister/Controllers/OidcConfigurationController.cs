using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace CocomeStore.Controllers
{
    /// <summary>
    /// class <c>OidcConfigurationController</c> provides the identity servers
    /// configuration
    /// </summary>
    public class OidcConfigurationController : Controller
    {

        public OidcConfigurationController(
            IClientRequestParametersProvider clientRequestParametersProvider
        )
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        [HttpGet]
        [Route("_configuration/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute]string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }
    }
}
