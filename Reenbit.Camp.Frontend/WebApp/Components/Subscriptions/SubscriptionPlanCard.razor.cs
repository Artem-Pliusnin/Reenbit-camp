using Domain.Models.Subscriptions;
using Domain.Requests.Subscriptions;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Components.Subscriptions;

public partial class SubscriptionPlanCard : ComponentBase
{
    [CascadingParameter(Name="UserSubscription")]
    public UserSubscriptionModel UserSubscription { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public SubscriptionPlanModel SubscriptionPlan { get; set; }
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    
    [Inject] 
    public ISubscriptionsService SubscriptionsService{ get; set; } = default!;
    
    private bool IsFree => SubscriptionPlan.MonthlyPrice == 0;

    private async void OnSelectPlan()
    {
        if (UserSubscription.SubscriptionPlan.Id == SubscriptionPlan.Id)
        {
            return;
        }
        
        var result = await SubscriptionsService
            .CreateCheckoutSessionAsync(
                new CreateCheckoutRequest(
                    SubscriptionPlan.Id,
                    Navigation.ToAbsoluteUri("payment-success").ToString(),
                    Navigation.BaseUri
                ));

        if (result.IsSuccess)
        {
            Navigation.NavigateTo(result.Value, forceLoad:true);
        }
    }
    
    private async void OnCancelPlan()
    {
        if (UserSubscription.SubscriptionPlan.Id != SubscriptionPlan.Id
            || IsFree)
        {
            return;
        }
        
        var result = await SubscriptionsService
            .CancelSubscriptionAsync();

        if (result.IsSuccess)
        {
            UserSubscription.CancelAtPeriodEnd = true;
            StateHasChanged();
        }
    }

    private bool CanSelectPlan()
    {
        return (!IsFree && UserSubscription.SubscriptionPlan.Id != SubscriptionPlan.Id);  
    } 
}