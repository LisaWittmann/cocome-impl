using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Authorization;

namespace CocomeStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly CocomeDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClaimManager _claimManager;

        public SeedController(
            CocomeDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ClaimManager claimManager
        )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _claimManager = claimManager;
        }

        [HttpGet]
        public async Task<ActionResult> CreateDefaultUsers()
        {
            string role_Administrator = "Administrator";
            string role_Cashier = "Kassierer";
            string role_Manager = "Filialleiter";

            if (await _roleManager.FindByNameAsync(role_Administrator) == null)
                await _roleManager.CreateAsync(new IdentityRole(role_Administrator));

            if (await _roleManager.FindByNameAsync(role_Cashier) == null)
                await _roleManager.CreateAsync(new IdentityRole(role_Cashier));

            if (await _roleManager.FindByNameAsync(role_Manager) == null)
                await _roleManager.CreateAsync(new IdentityRole(role_Manager));

            var addedUserList = new List<ApplicationUser>();

            var email_Admin = "admin@mail.com";
            if (await _userManager.FindByEmailAsync(email_Admin) == null)
            {
                var user_Admin = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = email_Admin,
                    Email = email_Admin,
                };

                await _userManager.CreateAsync(user_Admin, "MySecr3t$");

                await _userManager.AddToRoleAsync(user_Admin, role_Administrator);
                await _userManager.AddClaimsAsync(user_Admin, await _claimManager.GetClaims(user_Admin));


                user_Admin.EmailConfirmed = true;
                user_Admin.LockoutEnabled = false;
                addedUserList.Add(user_Admin);
            }

            var testStore = new Store()
            {
                Name = "Filiale Mustermann",
                City = "Mustertadt",
                PostalCode = 11111,
            };

            var email_Manager = "manager@mail.com";
            if (await _userManager.FindByEmailAsync(email_Manager) == null)
            {
                var user_Manager = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Max",
                    LastName = "Mustermann",
                    UserName = email_Manager,
                    Store = testStore,
                    Email = email_Manager
                };
                await _userManager.CreateAsync(user_Manager, "MySecr3t$");

                await _userManager.AddToRoleAsync(user_Manager, role_Manager);
                await _userManager.AddClaimsAsync(user_Manager, await _claimManager.GetClaims(user_Manager));;

                user_Manager.EmailConfirmed = true;
                user_Manager.LockoutEnabled = false;

                addedUserList.Add(user_Manager);
            }

            var email_Cashier = "kassierer@mail.com";
            if (await _userManager.FindByEmailAsync(email_Cashier) == null)
            {
                var user_Cashier = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Monika",
                    LastName = "Mustermann",
                    UserName = email_Cashier,
                    Store = testStore,
                    Email = email_Cashier
                };
                await _userManager.CreateAsync(user_Cashier, "MySecr3t$");

                await _userManager.AddToRoleAsync(user_Cashier, role_Cashier);
                await _userManager.AddClaimsAsync(user_Cashier, await _claimManager.GetClaims(user_Cashier));

                user_Cashier.EmailConfirmed = true;
                user_Cashier.LockoutEnabled = false;

                addedUserList.Add(user_Cashier);
            }

            if (addedUserList.Count > 0)
                await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                Users = addedUserList
            });
        }
    }
}
