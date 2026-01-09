using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.Identity.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FiapCloudGames.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                     .HasColumnName("Email")
                     .IsRequired();

                email.HasIndex(e => e.Address)
                   .IsUnique();
            });

            builder.OwnsOne(u => u.Password, password =>
            {
                password.Property(p => p.Value)
                        .HasColumnName("Password")
                        .IsRequired()
                        .HasMaxLength(256);
            });

            var roleConverter = new ValueConverter<Role, string>(
                role => role.Value,
                value => Role.Parse(value)
            );

            builder.Property(u => u.Role)
                   .HasConversion(roleConverter)
                   .HasColumnName("Role")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(150);

        }
    }
}
