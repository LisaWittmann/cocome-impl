﻿using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CocomeStore.Models
{
    public class CocomeDbContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<OrderElement> OrderElements { get; set; } 

        public DbSet<Order> Orders { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<SaleElement> SaleElements { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<StockItem> StockItems { get; set; }

        public CocomeDbContext(DbContextOptions<CocomeDbContext> options)
            : base(options)
        {
        }

        public CocomeDbContext()
        {
        }


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