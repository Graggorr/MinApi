using Microsoft.EntityFrameworkCore;
using WebStore.API.Infrastructure.Orders;
using WebStore.API.Infrastructure.Clients;
using WebStore.API.Domain;
using WebStore.Events.Orders;
using WebStore.Events.Clients;

namespace WebStore.API.Infrastructure
{
    public class WebStoreContext(DbContextOptions<WebStoreContext> options) : DbContext(options)
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientEvent> ClientEvents { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderEvent> OrderEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new ClientEventConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEventConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
