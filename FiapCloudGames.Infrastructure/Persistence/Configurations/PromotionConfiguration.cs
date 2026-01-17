using FiapCloudGames.Domain.PromotionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace FiapCloudGames.Infrastructure.Persistence.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.OwnsOne(p => p.Discount, discount =>
            {
                discount.Property(d => d.Value)
                    .IsRequired()
                    .HasColumnName("DiscountValue");

                discount.Property(d => d.Type)
                    .IsRequired()
                    .HasColumnName("DiscountType");
            });

            builder.Property(p => p.StartsAt)
                .IsRequired();
            builder.Property(p => p.EndsAt)
                .IsRequired(false);

            builder
            .HasMany(p => p.Games)
            .WithMany(g => g.Promotions);

        }
    }
}
