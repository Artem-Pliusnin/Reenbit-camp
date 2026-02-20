using AutoMapper;
using Domain.DTOs.Subscriptions;
using Domain.Entities;

namespace Application.Mapping;

public class SubscriptionPlanProfile : Profile
{
    public SubscriptionPlanProfile()
    {
        CreateMap<SubscriptionPlan, SubscriptionPlanDto>();
    }
}