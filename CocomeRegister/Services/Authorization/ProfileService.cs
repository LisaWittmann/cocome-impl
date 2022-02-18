using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace CocomeStore.Services.Authorization
{
    public class ProfileService : IProfileService
    {
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
