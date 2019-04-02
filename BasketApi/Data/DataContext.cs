using System;
using BasketApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;

namespace BasketApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var items = GetSeedItems();
            modelBuilder.Entity<Item>().HasData(items);
        }

        private Item[] GetSeedItems()
        {
            return new Item[]
            {
                new Item { Id = 1, Name = "Book", Price = 35.0m },
                new Item { Id = 2, Name = "CD", Price = 20.0m },
                new Item { Id = 3, Name = "DVD", Price = 14.0m }
            };
        }
    }
}
