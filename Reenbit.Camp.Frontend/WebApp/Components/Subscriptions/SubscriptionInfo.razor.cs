using Domain.Models.Subscriptions;
using Microsoft.AspNetCore.Components;

namespace WebApp.Components.Subscriptions;

public partial class SubscriptionInfo : ComponentBase
{
    [Parameter, EditorRequired]
    public UserSubscriptionModel Subscription { get; set; }
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    
    private bool IsFree => Subscription.SubscriptionPlan.Name == "Free";

    private void NavigateToPlans()
    {
        Navigation.NavigateTo("/subscription/plans");
    }
}