using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CocomeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CocomeStore.Models.Authorization;
using System.Threading.Tasks;
using CocomeStore.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Authorize(Policy = "enterprise")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly CocomeDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClaimManager _claimManager;

        public UserController(
            ILogger<UserController> logger,
            CocomeDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ClaimManager claimManager)
        {
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _claimManager = claimManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return _userManager.Users.Include(user => user.Store).ToArray();
        }

        [HttpGet]
        [Route("roles")]
        public ActionResult<IEnumerable<IdentityRole>> GetAllRoles()
        {
            return _roleManager.Roles.ToArray();
        }

        [HttpPost]
        public async Task<ApplicationUser> CreateNewUserAsync(ApplicationUser applicationUser, String password, String role)
        {
            applicationUser.SecurityStamp = Guid.NewGuid().ToString();
            await _userManager.CreateAsync(applicationUser, password);

            await _userManager.AddToRoleAsync(applicationUser, role);
            await _userManager.AddClaimsAsync(applicationUser, await _claimManager.GetClaimsAsync(applicationUser));

            _context.SaveChanges();
            return applicationUser;
        }

        [HttpPost]
        [Route("role")]
        public async Task<ApplicationUser> ChangeUserRoleAsync(String username, String role)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            await _userManager.AddToRoleAsync(user, role);
            await _userManager.AddClaimsAsync(user, await _claimManager.GetClaimsAsync(user));

            _context.SaveChanges();
            return user;
        }

    }
}
