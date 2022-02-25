using CocomeStore.Models.Authorization;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>UserTO</c> is a data transfer object for class
    /// <see cref="ApplicationUser"/>
    /// </summary>
    public class UserTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public Store Store { get; set; }

        public string[] Roles { get; set; }

        public string Password { get; set; }
    }
}
