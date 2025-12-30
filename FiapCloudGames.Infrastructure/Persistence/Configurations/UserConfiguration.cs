using FiapCloudGames.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            });
            builder.OwnsOne(u => u.Password, password =>
            {
                password.Property(p => p.Value)
                        .HasColumnName("Password")
                        .IsRequired()
                        .HasMaxLength(256);
            });
            builder.OwnsOne(u => u.Role, role =>
            {
                role.Property(r => r.Value)
                    .HasColumnName("Role")
                    .IsRequired();
            });
            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(150);

        }
    }
}
