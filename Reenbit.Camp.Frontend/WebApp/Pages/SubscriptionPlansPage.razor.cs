using Domain.Models.Subscriptions;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Pages;

public partial class SubscriptionPlansPage : ComponentBase
{
    [Inject]
    public ISubscriptionsService SubscriptionsService { get; set; } = default!;
    
    private List<SubscriptionPlanModel> SubscriptionPlans = new();

    private UserSubscriptionModel UserSubscription;
    
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        var userSubscriptionResult = await SubscriptionsService
            .GetCurrentUserSubscriptionAsync();

        if (userSubscriptionResult.IsSuccess)
        {
            UserSubscription = userSubscriptionResult.Value;
        }
        
        var result = await SubscriptionsService
            .GetSubscriptionPlansAsync();

        if (result.IsSuccess)
        {
            SubscriptionPlans = result.Value;
            isLoading = false;
        }
    }
}