using AutoMapper;
using Domain.DTOs.CardMembers;
using Domain.Entities;

namespace Application.Mapping;

public class CardMemberProfile: Profile
{
    public CardMemberProfile()
    {
        CreateMap<CardMember, CardMemberDto>();
    }
}