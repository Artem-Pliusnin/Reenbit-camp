using AutoMapper;
using Domain.Models.Subscriptions;
using Domain.Models.Users;
using Domain.Responses.Subscriptions;
using Domain.Responses.Users;

namespace Services.Mapping;

public class SubscriptionsProfile : Profile
{
    public SubscriptionsProfile()
    {
        CreateMap<SubscriptionPlanDto, SubscriptionPlanModel>();
        
        CreateMap<UserSubscriptionDto, UserSubscriptionModel>();
    }
}