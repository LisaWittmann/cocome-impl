using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Models
{
    public class ProduktDbContext : DbContext
    {
        public DbSet<Produkt> Produkts { get; set; }
        //public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        //{
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produkt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Beschreibung).HasMaxLength(1000);
                entity.Property(e => e.Preis);

                /*entity.HasData(new Produkt
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
                });*/

            });
        }
    }
}
