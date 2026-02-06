using Archivation.Function.Models.Etities;
using Archivation.Function.Models.DTos;
using AutoMapper;

namespace Archivation.Function.Services.Mapping;

public class ArchivationProfile : Profile
{
    public ArchivationProfile()
    {
        CreateMap<Board, BoardDto>();
        
        CreateMap<BoardMember, BoardMemberDto>();

        CreateMap<List, ListDto>();

        CreateMap<Card, CardDto>();
        
        CreateMap<CardAttachment, CardAttachmentDto>();
        
        CreateMap<CardLabel, CardLabelDto>();
        
        CreateMap<CardMember, CardMemberDto>();

        CreateMap<Comment, CommentDto>();
        
        CreateMap<Invitation, InvitationDto>();
        
        CreateMap<Label, LabelDto>();
    }
}