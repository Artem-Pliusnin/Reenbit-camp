namespace Domain.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int BoardsLimit { get; set; }
    
    public decimal MonthlyPrice { get; set; }
    
    public string StripeProductId { get; set; }
    
    public string StripePriceId { get; set; }
    
    public DateTime CreatedAt { get; set; }
}