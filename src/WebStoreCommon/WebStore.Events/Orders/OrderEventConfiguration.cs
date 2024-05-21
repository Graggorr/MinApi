using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Events.Orders
{
    public class OrderEventConfiguration : IEntityTypeConfiguration<OrderEvent>
    {
        public void Configure(EntityTypeBuilder<OrderEvent> builder)
        {
            builder.ToTable(nameof(OrderEvent));

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedNever().IsRequired();
            builder.Property(e => e.Price).HasColumnName("Price").IsRequired();
            builder.Property(e => e.Description).HasColumnName("Description").IsRequired();
            builder.Property(e => e.ClientId).HasColumnName("ClientId").IsRequired();
            builder.Property(e => e.Name).HasColumnName("Name").IsRequired();
            builder.Property(e => e.RouteKey).HasColumnName("RouteKey").IsRequired();
            builder.Property(e => e.QueueName).HasColumnName("QueueName").IsRequired();
            builder.Property(e => e.IsProcessed).HasColumnName("IsProcessed").IsRequired();
            builder.Property(e => e.CreationTimeUtc).HasColumnName("CreationTimeUtc").IsRequired();
        }
    }
}
