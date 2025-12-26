using AutoMapper;
using Domain.DTOs.Cards;
using Domain.Entities;

namespace Application.Mapping;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<Card, CardDto>();
        
        CreateMap<Card, CardInfoDto>();
    }
}