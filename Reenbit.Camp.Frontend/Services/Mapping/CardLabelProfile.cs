using AutoMapper;
using Domain.Models.Labels;
using Domain.Responses.Labels;

namespace Services.Mapping;

public class CardLabelProfile : Profile
{
    public CardLabelProfile()
    {
        CreateMap<CardLabelDto, CardLabelModel>().ReverseMap();
    }
}