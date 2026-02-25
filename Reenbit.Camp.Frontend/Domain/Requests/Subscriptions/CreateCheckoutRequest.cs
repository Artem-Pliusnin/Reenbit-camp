namespace Domain.Requests.Subscriptions;

public sealed record CreateCheckoutRequest(
    int PlanId,
    string SuccessUrl,
    string CancelUrl,
    string ChangeUrl);