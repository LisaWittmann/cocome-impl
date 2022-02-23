using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace CocomeStore.Services.Authorization
{
    /// <summary>
    /// class <c>ProdfileService</c> implements <see cref="IProfileService"/>
    /// to pass customize user information to the identitiy server
    /// </summary>
    public class ProfileService : IProfileService
    {
        /// <summary>
        /// method <c>GetProfileDataAsync</c> passes the users profile data
        /// to the indentity server and defines the data that are used in the
        /// generated jwt token
        /// </summary>
        /// <param name="context">profile data request of identity server</param>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);
            context.IssuedClaims.AddRange(roleClaims);

            var nameClaims = context.Subject.FindAll(JwtClaimTypes.Name);
            context.IssuedClaims.AddRange(nameClaims);

            var storeClaims = context.Subject.FindAll("store");
            context.IssuedClaims.AddRange(storeClaims);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
