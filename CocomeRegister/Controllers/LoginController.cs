using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CocomeStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CocomeDbContext _context;

        public LoginController(
            ILogger<LoginController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            CocomeDbContext context
        )
        {
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> CreateDefaultUsers()
        {
            string roleAdmin = "Administrator";
            if (await _roleManager.FindByNameAsync(roleAdmin) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleAdmin));
            }

            var addedUserList = new List<ApplicationUser>();

            var emailAdmin = "admin@mail.de";
            if (await _userManager.FindByNameAsync(emailAdmin) == null)
            {
                var userAdmin = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Email = emailAdmin,
                };
                await _userManager.CreateAsync(userAdmin, "P4S$worD!");
                await _userManager.AddToRoleAsync(userAdmin, roleAdmin);

                userAdmin.EmailConfirmed = true;
                userAdmin.LockoutEnabled = false;
                addedUserList.Add(userAdmin);
            }

            if (addedUserList.Count > 0)
            {
                await _context.SaveChangesAsync();
            }
            return new JsonResult(new
            {
                Users = addedUserList
            });
      
        }
    }
}
