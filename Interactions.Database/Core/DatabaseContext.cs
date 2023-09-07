using Interactions.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interactions.Database.Core
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
            Books = Set<BookEntity>();
            Authors = Set<AuthorEntity>();
        }

        public DbSet<BookEntity> Books { get; }
        public DbSet<AuthorEntity> Authors { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region book

            var bookEntity = modelBuilder.Entity<BookEntity>();

            bookEntity.HasKey(e => e.Id);
            bookEntity.Navigation(e => e.Author)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            bookEntity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id")
                .IsRequired();
            bookEntity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired();
            bookEntity.Property(e => e.Description)
                .HasColumnName("description");

            bookEntity.ToTable("Books");

            #endregion

            #region author

            var authorEntity = modelBuilder.Entity<AuthorEntity>();

            authorEntity.HasKey(e => e.Id);
            authorEntity.HasMany(e => e.Books)
                .WithOne(e => e.Author)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            authorEntity.Navigation(e => e.Books)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            authorEntity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired();
            authorEntity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id")
                .IsRequired();

            authorEntity.ToTable("Authors");


            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
