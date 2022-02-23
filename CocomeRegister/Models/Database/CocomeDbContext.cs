using CocomeStore.Models.Authorization;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CocomeStore.Models.Database
{
    public class CocomeDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public CocomeDbContext(
           DbContextOptions options,
           IOptions<OperationalStoreOptions> operationalStoreOptions)
           : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<OrderElement> OrderElements { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<SaleElement> SaleElements { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<StockItem> StockItems { get; set; }

        public DbSet<ExchangeElement> ExchangeElements { get; set; }

        public DbSet<StockExchange> StockExchanges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Cocome.db");
        }
    }
}
