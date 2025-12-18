using AutoMapper;
using Domain.Models.Cards;
using Domain.Responses.Cards;

namespace Services.Mapping;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<CardDto, CardModel>();
    }
}