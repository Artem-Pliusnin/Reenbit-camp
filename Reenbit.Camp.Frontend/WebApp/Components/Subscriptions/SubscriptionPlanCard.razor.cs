using Domain.Models.Subscriptions;
using Domain.Requests.Subscriptions;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Components.Subscriptions;

public partial class SubscriptionPlanCard : ComponentBase
{
    [Parameter, EditorRequired]
    public SubscriptionPlanModel SubscriptionPlan { get; set; }
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    
    [Inject] 
    public ISubscriptionsService SubscriptionsService{ get; set; } = default!;
    
    private bool IsFree => SubscriptionPlan.MonthlyPrice == 0;

    private async void OnSelectPlan()
    {
        var result = await SubscriptionsService
            .CreateCheckoutSessionAsync(
                new CreateCheckoutRequest(
                    SubscriptionPlan.Id,
                    Navigation.ToAbsoluteUri("error").ToString(),
                    Navigation.BaseUri
                ));

        if (result.IsSuccess)
        {
            Navigation.NavigateTo(result.Value, forceLoad:true);
        }
    }
}