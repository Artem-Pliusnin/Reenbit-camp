using Domain.Models.Subscriptions;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Components.Subscriptions;

public partial class SubscriptionInfo : ComponentBase
{
    [Parameter, EditorRequired]
    public UserSubscriptionModel Subscription { get; set; }
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    
    [Inject] 
    public ISubscriptionsService SubscriptionsService{ get; set; } = default!;
    
    private bool IsFree => Subscription.SubscriptionPlan.Name == "Free";

    private void NavigateToPlans()
    {
        Navigation.NavigateTo("/subscription/plans");
    }

    private async Task CancelPlan()
    {
        if (IsFree && Subscription.CancelAtPeriodEnd)
        {
            return;
        }
        
        var result = await SubscriptionsService
            .CancelSubscriptionAsync();

        if (result.IsSuccess)
        {
            Subscription.CancelAtPeriodEnd = true;
            StateHasChanged();
        }
    }
}