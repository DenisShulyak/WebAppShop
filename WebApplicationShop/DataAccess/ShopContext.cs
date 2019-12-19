using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationShop.Models;

namespace WebApplicationShop.DataAccess
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Name = "Телефон", Description = "Samsung", Cost = 10000 },
                new Product { Name = "Наушники", Description = "Китай", Cost = 500 },
                new Product { Name = "Ремень", Description = "Кожа", Cost = 1000 }
                );

            modelBuilder.Entity<Basket>().HasData(
                new Basket { Products = new List<Product>() });
        }
    }
}
