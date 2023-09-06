using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Interactions.Database.Entities;

namespace Interactions.Database.Configuration
{
    internal class BookConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("Id");
            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name");
            builder.Property(e => e.Description)
               .IsRequired()
               .HasColumnName("Description");
            builder.Property(e => e.Authors)
                .IsRequired()
                .HasColumnName("Authors");
        }
    }
}
