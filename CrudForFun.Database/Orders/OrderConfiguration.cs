using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebStore.Domain;

namespace WebStore.Infrastructure.Orders
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasColumnName("Name");
            builder.Property(e => e.Id).HasColumnName("ClientId");
            builder.Property(e => e.Price).HasColumnName("PhoneNumber");
            builder.Property(e => e.Description).HasColumnName("Email");
        }
    }
}
