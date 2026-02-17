using Domain.Requests.Labels;
using Domain.Requests.Subscriptions;
using Domain.Responses.Labels;
using Domain.Responses.Subscriptions;
using Domain.Shared;
using Refit;

namespace Services.API;

public interface ISubscriptionsApi
{
    [Get("/Subscriptions/plans")]
    Task<ApiResponse<List<SubscriptionPlanDto>>> GetSubscriptionPlansAsync();

    [Post("/Subscriptions/create-checkout")]
    Task<ApiResponse<string>> CreateCheckoutSessionAsync(
        [Body] CreateCheckoutRequest request);
    
    [Post("/Subscriptions/cancel")]
    Task<ApiResponse<object>> CancelSubscriptionAsync();
}
