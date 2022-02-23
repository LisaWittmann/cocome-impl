using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// endpoint to request the applications OpenID identity server configuration
        /// </summary>
        /// <param name="clientId">registered clientname for OpenID</param>
        /// <returns>OpenId configuration parameters</returns>
        /// <response code="200">identity server configuration</response>
        [HttpGet]
        [Route("_configuration/{clientId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetClientRequestParameters([FromRoute]string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }
    }
}
