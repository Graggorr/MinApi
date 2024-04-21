using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebStore.Domain;

namespace WebStore.Infrastructure.Clients
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(e => e.PhoneNumber);
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasMany(e => e.Orders).WithMany(e => e.Clients);
            builder.Property(e => e.Name).HasColumnName("Name");
            builder.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(e => e.Email).HasColumnName("Email");

        }
    }
}
