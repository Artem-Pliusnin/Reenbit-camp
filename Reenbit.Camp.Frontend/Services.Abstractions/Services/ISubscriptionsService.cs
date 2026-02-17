using Domain.Models.Subscriptions;
using Domain.Requests.Subscriptions;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ISubscriptionsService
{
    Task<Result<List<SubscriptionPlanModel>>> GetSubscriptionPlansAsync();
    
    Task<Result<string>> CreateCheckoutSessionAsync(CreateCheckoutRequest request);
    
    Task<Result<object>> CancelSubscriptionAsync();
}