﻿using Microsoft.EntityFrameworkCore;
using WebStore.Domain;
using WebStore.Infrastructure.Orders;
using WebStore.Infrastructure.Clients;

namespace WebStore.Infrastructure
{
    public class WebStoreContext : DbContext
    {
        public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Order>().ToTable("Orders");

            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
