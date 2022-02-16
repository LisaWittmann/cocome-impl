using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CocomeStore.Models.Authorization
{
    public class JwtHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public JwtHandler(
            UserManager<ApplicationUser> userManager
        )
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Claim>> GetClaims(ApplicationUser user)
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
