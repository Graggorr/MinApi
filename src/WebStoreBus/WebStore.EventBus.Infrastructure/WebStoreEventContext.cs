using Microsoft.EntityFrameworkCore;
using WebStore.EventBus.Events;

namespace WebStore.EventBus.Infrastructure
{
    public class WebStoreEventContext : DbContext
    {
        public WebStoreEventContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ClientEvent> ClientEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientEventConfiguration());
        }
    }
}
