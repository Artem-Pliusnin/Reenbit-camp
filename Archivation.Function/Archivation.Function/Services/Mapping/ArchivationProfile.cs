using Archivation.Function.Models.Etities;
using Archivation.Function.Models.DTos;
using AutoMapper;

namespace Archivation.Function.Services.Mapping;

public class ArchivationProfile : Profile
{
    public ArchivationProfile()
    {
        CreateMap<Board, BoardDto>().ReverseMap();
        
        CreateMap<BoardMember, BoardMemberDto>().ReverseMap();

        CreateMap<List, ListDto>().ReverseMap();

        CreateMap<Card, CardDto>().ReverseMap();
        
        CreateMap<CardAttachment, CardAttachmentDto>().ReverseMap();
        
        CreateMap<CardLabel, CardLabelDto>().ReverseMap();
        
        CreateMap<CardMember, CardMemberDto>().ReverseMap();

        CreateMap<Comment, CommentDto>().ReverseMap();
        
        CreateMap<Invitation, InvitationDto>().ReverseMap();
        
        CreateMap<Label, LabelDto>().ReverseMap();
    }
}