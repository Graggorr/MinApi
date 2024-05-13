using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebStore.EventBus.Events;

namespace WebStore.EventBus.Infrastructure
{
    public class WebStoreEventContext : DbContext
    {
        public WebStoreEventContext(): base()
        {
            Database.EnsureCreated();
        }

        public DbSet<ClientEvent> ClientEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=webstore;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientEventConfiguration());
        }
    }
}
