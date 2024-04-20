using WebStore.Database.Configuration;
using WebStore.Database.Entities;
using Microsoft.EntityFrameworkCore;
using WebStore.Infrastructure.Entities;
using WebStore.Infrastructure.Configuration;

namespace WebStore.Database
{
    public class WebStoreContext: DbContext
    {
        public WebStoreContext(DbContextOptions<WebStoreContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<PhoneEntity> Phones { get; set; }
        public DbSet<LaptopEntity> Laptops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemEntity>().ToTable("Items");
            modelBuilder.Entity<PhoneEntity>().ToTable("Phones");
            modelBuilder.Entity<LaptopEntity>().ToTable("Laptops");

            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneConfiguration());
            modelBuilder.ApplyConfiguration(new LaptopConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
