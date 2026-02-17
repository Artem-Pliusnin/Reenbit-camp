namespace Domain.Models.Subscriptions;

public class UserSubscriptionModel
{
    public int Id { get; set; }
    
    public DateTime? CurrentPeriodEnd { get; set; }

    public bool CancelAtPeriodEnd { get; set; }
    
    public SubscriptionPlanModel SubscriptionPlan { get; set; }
}