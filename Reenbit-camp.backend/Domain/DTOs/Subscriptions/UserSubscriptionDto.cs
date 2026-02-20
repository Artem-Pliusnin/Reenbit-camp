using Domain.Entities;
using Domain.Enums;

namespace Domain.DTOs.Subscriptions;

public class UserSubscriptionDto
{
    public int Id { get; set; }
    
    public DateTime? CurrentPeriodEnd { get; set; }

    public bool CancelAtPeriodEnd { get; set; }
    
    public SubscriptionPlanDto SubscriptionPlan { get; set; }
}