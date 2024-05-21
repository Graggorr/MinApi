using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebStore.API.Domain;

namespace WebStore.API.Infrastructure.Orders
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order));

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd().IsRequired();
            builder.Property(e => e.Name).HasColumnName("Name").IsRequired();
            builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
            builder.Property(e => e.Price).HasColumnName("Price").IsRequired();
            builder.Property(e => e.Description).HasColumnName("Description").IsRequired();
        }
    }
}