using WebStore.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebStore.Infrastructure.Configuration
{
    internal class PhoneConfiguration : IEntityTypeConfiguration<PhoneEntity>
    {
        public void Configure(EntityTypeBuilder<PhoneEntity> builder)
        {
            builder.HasKey(e => e.ItemId);
            builder.Property(e => e.Name).HasColumnName("Name");
            builder.Property(e => e.ItemId).HasColumnName("ItemId");
            builder.Property(e => e.Producer).HasColumnName("Producer");
            builder.Property(e => e.Country).HasColumnName("Country");
        }
    }
}
