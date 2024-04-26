using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebStore.Domain;

namespace WebStore.Infrastructure.Clients
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable(nameof(Client));

            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.PhoneNumber).IsUnique();

            builder.HasMany(e => e.Orders).WithMany(e => e.Clients).UsingEntity(
                e => e.HasOne(typeof(Client)).WithMany().HasForeignKey($"{typeof(Client)}ForeignKey"),
                x => x.HasOne(typeof(Order)).WithMany().HasForeignKey($"{typeof(Order)}ForeignKey"));

            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedNever().IsRequired();
            builder.Property(e => e.Name).HasColumnName("Name").IsRequired();
            builder.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber").IsRequired();
            builder.Property(e => e.Email).HasColumnName("Email").IsRequired();
        }
    }
}
