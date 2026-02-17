using AutoMapper;
using Domain.DTOs.Subscriptions;
using Domain.Entities;

namespace Application.Mapping;

public class UserSubscriptionProfile : Profile
{
    public UserSubscriptionProfile()
    {
        CreateMap<UserSubscription, UserSubscriptionDto>();
    }
}