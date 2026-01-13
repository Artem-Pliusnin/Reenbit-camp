using AutoMapper;
using Domain.Models.CardMembers;
using Domain.Responses.CardMembers;

namespace Services.Mapping;

public class CardMemberProfile : Profile
{
    public CardMemberProfile()
    {
        CreateMap<CardMemberDto, CardMemberModel>().ReverseMap();
    }
}