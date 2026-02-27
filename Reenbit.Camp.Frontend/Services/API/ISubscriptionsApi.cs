using Domain.Requests.Labels;
using Domain.Requests.Subscriptions;
using Domain.Responses.Labels;
using Domain.Responses.Subscriptions;
using Domain.Shared;
using Refit;

namespace Services.API;

public interface ISubscriptionsApi
{
    [Get("/Subscriptions/current")]
    Task<ApiResponse<UserSubscriptionDto>> GetCurrentUserSubscriptionAsync();
    
    [Get("/Subscriptions/plans")]
    Task<ApiResponse<List<SubscriptionPlanDto>>> GetSubscriptionPlansAsync();

    [Post("/Subscriptions/create-checkout")]
    Task<ApiResponse<string>> CreateCheckoutSessionAsync(
        [Body] CreateCheckoutRequest request);
    
    [Post("/Subscriptions/cancel")]
    Task<ApiResponse<object>> CancelSubscriptionAsync();
    
    [Post("/Subscriptions/resume")]
    Task<ApiResponse<object>> ResumeSubscriptionAsync();
}
