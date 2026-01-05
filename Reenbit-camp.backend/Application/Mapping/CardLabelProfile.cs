using AutoMapper;
using Domain.DTOs.CardLabels;
using Domain.Entities;

namespace Application.Mapping;

public class CardLabelProfile : Profile
{
    public CardLabelProfile()
    {
        CreateMap<CardLabel, CardLabelDto>();
    }
}