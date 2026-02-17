namespace Domain.Responses.Subscriptions;

public sealed record SubscriptionPlanDto(
    int Id, 
    string Name, 
    int BoardsLimit, 
    decimal MonthlyPrice);