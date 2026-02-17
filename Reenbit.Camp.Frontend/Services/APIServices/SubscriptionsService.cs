using AutoMapper;
using Domain.Models.Subscriptions;
using Domain.Requests.Subscriptions;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class SubscriptionsService : ISubscriptionsService
{
    private readonly ISubscriptionsApi _subscriptionsApi;
    private readonly IMapper _mapper;

    public SubscriptionsService(ISubscriptionsApi subscriptionsApi, IMapper mapper)
    {
        _subscriptionsApi = subscriptionsApi;
        _mapper = mapper;
    }
    
    public async Task<Result<List<SubscriptionPlanModel>>> GetSubscriptionPlansAsync()
    {
        var response = await _subscriptionsApi.GetSubscriptionPlansAsync();
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<SubscriptionPlanModel>>(content));
    }

    public async Task<Result<string>> CreateCheckoutSessionAsync(CreateCheckoutRequest request)
    {
        var response = await _subscriptionsApi.CreateCheckoutSessionAsync(request);

        return response.HandleResult();
    }

    public async Task<Result<object>> CancelSubscriptionAsync()
    {
        var response = await _subscriptionsApi.CancelSubscriptionAsync();

        return response.HandleResult();
    }
}