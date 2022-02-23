namespace CocomeStore.Models.Authorization
{
    /// <summary>
    /// static class <c>ApplicationRoles</c> defines the applications user roles
    /// for authentication to use as <see cref="Microsoft.AspNetCore.Identity.IdentityRole"/>
    /// </summary>
    public static class ApplicationRoles
    {
        public static readonly string Admin = "Administrator";
        public static readonly string Manager = "Filialleiter";
        public static readonly string Cashier = "Kassierer";
    }
}
