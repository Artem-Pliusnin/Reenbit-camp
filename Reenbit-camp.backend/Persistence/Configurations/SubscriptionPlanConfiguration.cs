using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converters;

namespace Persistence.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("SubscriptionPlan");

        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.Id)
            .HasColumnName("id");

        builder.Property(sp => sp.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(sp => sp.BoardsLimit)
            .HasColumnName("board_limit")
            .IsRequired();
        
        builder.Property(sp => sp.IsAiAssistantAvailable)
            .HasColumnName("is_ai_assistant_available")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.Property(sp => sp.IsNameHighlighted)
            .HasColumnName("is_name_highlighted")
            .HasDefaultValue(false)
            .IsRequired();


        builder.Property(sp => sp.MonthlyPrice)
            .HasColumnName("monthly_price")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(sp => sp.StripeProductId)
            .HasColumnName("stripe_product_id")
            .HasMaxLength(255);

        builder.Property(sp => sp.StripePriceId)
            .HasColumnName("stripe_price_id")
            .HasMaxLength(255);

        builder.Property(sp => sp.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValue(DateTime.UtcNow)
            .HasConversion(UtcDateTimeConverters.NonNullable)
            .IsRequired();
            
        builder.HasIndex(sp => sp.Name)
            .IsUnique();
    }
}