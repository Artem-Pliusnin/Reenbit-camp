using AutoMapper;
using Domain.DTOs.Invitations;
using Domain.Entities;

namespace Application.Mapping;

public class InvitationProfile : Profile
{
    public InvitationProfile()
    {
        CreateMap<Invitation, InvitationDto>()
            .ForMember(dest => dest.SentDate, opt => opt.MapFrom(src => src.CreatedAt));
    }
}