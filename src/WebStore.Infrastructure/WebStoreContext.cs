using Microsoft.EntityFrameworkCore;
using WebStore.Domain;
using WebStore.Infrastructure.Orders;
using WebStore.Infrastructure.Clients;
using WebStore.Infrastructure.RabbitMq.Events;

namespace WebStore.Infrastructure
{
    public class WebStoreContext : DbContext
    {
        public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientEvent> ClientEvents { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new ClientEventConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
