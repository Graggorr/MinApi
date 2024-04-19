using WebStore.Database.Configuration;
using WebStore.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Database
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<PhoneEntity> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemEntity>().ToTable("Items");
            modelBuilder.Entity<PhoneEntity>().ToTable("Books");

            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
