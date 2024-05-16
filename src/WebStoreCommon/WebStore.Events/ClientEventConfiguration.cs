using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Events
{
    public class ClientEventConfiguration : IEntityTypeConfiguration<ClientEvent>
    {
        public void Configure(EntityTypeBuilder<ClientEvent> builder)
        {
            builder.ToTable(nameof(ClientEvent));

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedNever().IsRequired();
            builder.Property(e => e.Name).HasColumnName("Name").IsRequired();
            builder.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber").IsRequired();
            builder.Property(e => e.Email).HasColumnName("Email").IsRequired();
            builder.Property(e => e.Orders).HasColumnName("Orders").IsRequired();
            builder.Property(e => e.RouteKey).HasColumnName("RouteKey").IsRequired();
            builder.Property(e => e.QueueName).HasColumnName("QueueName").IsRequired();
            builder.Property(e => e.IsProcessed).HasColumnName("IsProcessed").IsRequired();
            builder.Property(e => e.CreationTimeUtc).HasColumnName("CreationTimeUtc").IsRequired();
        }
    }
}
