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
using CocomeStore.Models.Transfer;
using CocomeStore.Services.Mapping;

namespace CocomeStore.Controllers
{
    [ApiController]
    [Authorize(Policy = "enterprise")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly IModelMapper _mapper;
        private readonly CocomeDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClaimManager _claimManager;

        public UserController(
            ILogger<UserController> logger,
            IModelMapper mapper,
            CocomeDbContext context,
            UserManager<ApplicationUser> userManager,
            ClaimManager claimManager)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _claimManager = claimManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTO>>> GetAllUsers()
        {
            var users = new List<UserTO>();
            foreach(var appUser in _userManager.Users)
            {
                var user = new UserTO()
                {
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    Email = appUser.Email
                };
                if (appUser.StoreId != null)
                {
                    user.Store = await _context.Stores.FindAsync(appUser.StoreId);
                }
                user.Roles = await _userManager.GetRolesAsync(appUser);
                users.Add(user);
            }
            return users.ToArray();
        }

        [HttpPost]
        public async Task<ApplicationUser> CreateNewUserAsync(UserTO userTO, string password)
        {
            var applicationUser = _mapper.CreateApplicationUser(userTO);

            await _userManager.CreateAsync(applicationUser, password);
            if (userTO.Roles.Count() > 0)
            {
                await _userManager.AddToRolesAsync(applicationUser, userTO.Roles);
            }
            await _userManager.AddClaimsAsync(applicationUser, await _claimManager.GetClaimsAsync(applicationUser));

            await _context.SaveChangesAsync();
            return applicationUser;
        }

        [HttpPost]
        [Route("role")]
        public async Task<ApplicationUser> ChangeUserRoleAsync(string username, IEnumerable<string> roles)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);

            await _userManager.AddToRolesAsync(user, roles);
            await _userManager.AddClaimsAsync(user, await _claimManager.GetClaimsAsync(user));

            await _context.SaveChangesAsync();
            return user;
        }

    }
}
