using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebStore.API.Domain;

namespace WebStore.API.Infrastructure.Clients
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable(nameof(Client));

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.PhoneNumber).IsUnique();

            builder.HasMany(e => e.Orders).WithOne(e => e.Client).HasForeignKey(e => e.ClientId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedNever().IsRequired();
            builder.Property(e => e.Name).HasColumnName("Name").IsRequired();
            builder.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber").IsRequired();
            builder.Property(e => e.Email).HasColumnName("Email").IsRequired();
        }
    }
}
