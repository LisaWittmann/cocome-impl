using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CocomeStore.Models.Authorization;
using System.Threading.Tasks;
using CocomeStore.Models.Database;
using CocomeStore.Models.Transfer;
using CocomeStore.Services.Mapping;
using Microsoft.AspNetCore.Http;

namespace CocomeStore.Controllers
{
    /// <summary>
    /// class <c>UserController</c> provided REST endpoints for the application
    /// administrator to add new user, update store relations and indentity roles
    /// and requires authorization of enterprise policy
    /// </summary>
    [ApiController]
    [Authorize(Policy = "enterprise")]
    [Route("api/[controller]")]
    [Produces("application/json")]
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

        /// <summary>
        /// endpoint to request all application users from the database
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/user
        ///   
        /// </remarks>
        /// <response code="200">returns all user entries</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserTO>>> GetAllUsers()
        {
            _logger.LogInformation("reqesting all applications users");
            var users = new List<UserTO>();
            foreach(var appUser in _userManager.Users)
            {
                users.Add(await CreateUserTO(appUser));
            }
            return users.ToArray();
        }

        /// <summary>
        /// endpoint to create a new application user
        /// </summary>
        /// <param name="userTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/user
        ///     {
        ///         "firstName": "Erika",
        ///         "lastName": "Mustermann",
        ///         "email": "erikam@mail.com",
        ///         "store":
        ///             {
        ///                 "id": 1,
        ///                 "name": "Filiale Mustermann",
        ///             },
        ///         "roles": ["Kassierer"],
        ///     }
        ///   
        /// </remarks>
        /// <response code="200">user was successfully created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ApplicationUser> CreateUserAsync(UserTO userTO)
        {
            var applicationUser = _mapper.CreateApplicationUser(userTO);
            applicationUser.EmailConfirmed = true;

            await _userManager.CreateAsync(applicationUser, userTO.Password);
            if (userTO.Roles != null && userTO.Roles.Any())
            {
                await _userManager.AddToRolesAsync(applicationUser, userTO.Roles);
            }
            await _userManager.AddClaimsAsync(applicationUser, await _claimManager.GetClaimsAsync(applicationUser));

            await _context.SaveChangesAsync();
            _logger.LogInformation("registered new user {}", applicationUser.UserName);
            return applicationUser;
        }

        /// <summary>
        /// endpoint to modify a user with the given transfer objects username
        /// </summary>
        /// <param name="userTO">user transfer object containing the new data</param>
        /// <returns>modified user entry as tranfer object</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/user
        ///     {
        ///         "firstName": "Max",
        ///         "lastName": "Mustermann",
        ///         "email": "manager@mail.com",
        ///         "store":
        ///             {
        ///                 "id": 1,
        ///                 "name": "Filiale Mustermann",
        ///             },
        ///         "roles": ["Manager"],
        ///     }
        ///   
        /// </remarks>
        /// <response code="200">user was successfully updated</response>
        /// <response code="404">user entry was not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserTO>> UpdateUserAsync(UserTO userTO)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(userTO.Email);
            if (user == null)
            {
                return NotFound();
            }
            user.StoreId = userTO.Store.Id;

            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.RemoveClaimsAsync(user, await _userManager.GetClaimsAsync(user));
            await _userManager.AddToRolesAsync(user, userTO.Roles);
            await _userManager.AddClaimsAsync(user, await _claimManager.GetClaimsAsync(user));

            _logger.LogInformation("updated user {}", user.UserName);

            await _context.SaveChangesAsync();
            return await CreateUserTO(user);
        }

        /// <summary>
        /// method <c>CreateUserTO</c> creates a new user transfer object based
        /// on an application user object
        /// </summary>
        /// <param name="appUser">application user to convert for transfer</param>
        /// <returns>new instance of a user transfer object</returns>
        private async Task<UserTO> CreateUserTO(ApplicationUser appUser)
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
            user.Roles = (await _userManager.GetRolesAsync(appUser)).ToArray();
            return user;
        }
    }
}
