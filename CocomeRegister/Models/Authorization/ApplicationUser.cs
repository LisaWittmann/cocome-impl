using System;
using Microsoft.AspNetCore.Identity;
namespace CocomeStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public int? StoreId { get; set; }

        [PersonalData]
        public Store Store { get; set; }
    }
}
