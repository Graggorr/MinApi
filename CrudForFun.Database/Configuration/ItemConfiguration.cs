using WebStore.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebStore.Database.Configuration
{
    internal class ItemConfiguration : IEntityTypeConfiguration<ItemEntity>
    {
        public void Configure(EntityTypeBuilder<ItemEntity> builder)
        {
            builder.HasKey(e => e.ItemId);
            builder.HasOne(e => e.Phone).WithOne(e => e.Item).HasForeignKey<ItemEntity>(e => e.ItemId).OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.Phone).UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Property(e => e.ItemId).ValueGeneratedOnAdd().HasColumnName("ItemId");
            builder.Property(e => e.Name).HasColumnName("Name");
        }
    }
}
