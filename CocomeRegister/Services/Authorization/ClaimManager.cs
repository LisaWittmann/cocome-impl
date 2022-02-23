using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CocomeStore.Models.Authorization
{
    /// <summary>
    /// class <c>ClaimManager</c> manages the AspNetUserClaims for Authorization
    /// </summary>
    public class ClaimManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// method <c>GetClaimsAsync</c> generates user claims based on their given
        /// identity role and the store they belong to
        /// otherwise the custom userdata cannot be used for the jwt token
        /// </summary>
        /// <param name="user">application user to generate claims for</param>
        /// <returns></returns>
        public async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

           if (user.StoreId != null)
            {
                claims.Add(new Claim("store", user.StoreId.ToString()));
            }

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
