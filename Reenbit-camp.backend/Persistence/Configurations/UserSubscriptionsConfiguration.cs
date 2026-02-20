using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converters;

namespace Persistence.Configurations;

public class UserSubscriptionsConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder.ToTable("UserSubscriptions");

        builder.HasKey(us => us.Id);

        builder.Property(us => us.Id)
            .HasColumnName("id");

        builder.Property(us => us.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(us => us.SubscriptionPlanId)
            .HasColumnName("subscription_plan_id")
            .IsRequired();

        builder.Property(us => us.StripeCustomerId)
            .HasColumnName("stripe_customer_id")
            .HasMaxLength(255);

        builder.Property(us => us.StripeSubscriptionId)
            .HasColumnName("stripe_subscription_id")
            .HasMaxLength(255);
        
        builder.Property(us => us.SubscriptionStatus)
            .HasColumnName("status_id")
            .HasConversion<int>();

        builder.Property(us => us.CurrentPeriodEnd)
            .HasColumnName("current_period_end")
            .HasConversion(UtcDateTimeConverters.NonNullable);
        
        builder.Property(us => us.CancelAtPeriodEnd)
            .HasColumnName("cancel_at_period_end")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.HasOne(us => us.User)
            .WithOne(u => u.Subscription)
            .HasForeignKey<UserSubscription>(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(us => us.SubscriptionPlan)
            .WithMany()
            .HasForeignKey(us => us.SubscriptionPlanId)
            .OnDelete(DeleteBehavior.SetNull);
            
        builder.HasIndex(us => us.UserId)
            .IsUnique();
    }
}