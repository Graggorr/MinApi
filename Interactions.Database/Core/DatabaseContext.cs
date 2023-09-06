using Interactions.Database.Configuration;
using Interactions.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interactions.Database.Core
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            Books = Set<BookEntity>();
            Authors = Set<AuthorEntity>();
        }

        public DbSet<BookEntity> Books { get; }
        public DbSet<AuthorEntity> Authors { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookEntity>().ToTable("Books");
            modelBuilder.Entity<AuthorEntity>().ToTable("Authors");

            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
