using Restoration.Function.Models.Etities;
using Restoration.Function.Models.DTos;
using AutoMapper;

namespace Restoration.Function.Services.Mapping;

public class ArchivationProfile : Profile
{
    public ArchivationProfile()
    {
        CreateMap<BoardDto, Board>();
        
        CreateMap<BoardMemberDto, BoardMember>();

        CreateMap<ListDto, List>();

        CreateMap<CardDto, Card>();
        
        CreateMap<CardAttachmentDto, CardAttachment>();
        
        CreateMap<CardLabelDto, CardLabel>();
        
        CreateMap<CardMemberDto, CardMember>();

        CreateMap<CommentDto, Comment>();
        
        CreateMap<InvitationDto, Invitation>();
        
        CreateMap<LabelDto, Label>();
    }
}