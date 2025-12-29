using AutoMapper;
using Domain.Models.Invitations;
using Domain.Responses.Invitations;

namespace Services.Mapping;

public class InvitationProfile : Profile
{
    public InvitationProfile()
    {
        CreateMap<InvitationDto, InvitationModel>();
    }
}