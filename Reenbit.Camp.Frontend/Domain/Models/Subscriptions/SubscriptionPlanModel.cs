namespace Domain.Models.Subscriptions;

public class SubscriptionPlanModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int BoardsLimit { get; set; }
    
    public bool IsAiAssistantAvailable { get; set; }
    
    public bool IsNameHighlighted { get; set; }
    
    public decimal MonthlyPrice { get; set; }
}