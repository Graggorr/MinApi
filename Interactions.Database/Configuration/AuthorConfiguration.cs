using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Interactions.Database.Entities;

namespace Interactions.Database.Configuration
{
    internal class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(e => e.Books)
                .WithOne()
                .HasForeignKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("Id");
            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name");
            builder.Property(e => e.Books)
                .IsRequired()
                .HasColumnName("Books");
        }
    }
}
