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

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<SaleElement> SaleElements { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<StockItem> StockItems { get; set; }

       


        //public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        //{
        //}

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produkt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Beschreibung).HasMaxLength(1000);
                entity.Property(e => e.Preis);

                entity.HasData(new Produkt
                {
                    Provider = "Cookies",
                    UserId = 1,
                    Email = "bob@admonex.com",
                    Username = "bob",
                    Password = "pizza",
                    Firstname = "Bob",
                    Lastname = "Tester",
                    Mobile = "800-555-1212",
                    Roles = "Admin"
                });

            });
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Cocome.db");
        }
    }
}
