using Microsoft.AspNetCore.Identity;

namespace CocomeStore.Models.Authorization
{
    /// <summary>
    /// class <c>ApplicationUser</c> overwrites the default implementation of
    /// ASP identity's <see cref="IdentityUser"/>
    /// </summary>
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
