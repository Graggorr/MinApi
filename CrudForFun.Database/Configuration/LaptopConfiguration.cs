using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebStore.Infrastructure.Entities;

namespace WebStore.Infrastructure.Configuration
{
    public class LaptopConfiguration: IEntityTypeConfiguration<LaptopEntity>
    {
        public void Configure(EntityTypeBuilder<LaptopEntity> builder)
        {
            builder.HasKey(e => e.ItemId);
            builder.Property(e => e.Name).HasColumnName("Name");
            builder.Property(e => e.ItemId).HasColumnName("ItemId");
            builder.Property(e => e.Producer).HasColumnName("Producer");
            builder.Property(e => e.Country).HasColumnName("Country");
        }
    }
}
