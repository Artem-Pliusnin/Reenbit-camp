using Domain.Enums;

namespace Domain.Entities;

public class UserSubscription
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SubscriptionPlanId { get; set; }
    
    public string? StripeCustomerId { get; set; }
    
    public string? StripeSubscriptionId { get; set; }
    
    public SubscriptionStatus SubscriptionStatus { get; set; }
    
    public DateTime? CurrentPeriodEnd { get; set; }

    public bool CancelAtPeriodEnd { get; set; }
    
    public User User { get; set; }
    
    public SubscriptionPlan SubscriptionPlan { get; set; }
}