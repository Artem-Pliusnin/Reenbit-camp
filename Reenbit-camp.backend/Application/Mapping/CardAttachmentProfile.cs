using AutoMapper;
using Domain.DTOs.CardAttachments;
using Domain.Entities;

namespace Application.Mapping;

public class CardAttachmentProfile : Profile
{
    public CardAttachmentProfile()
    {
        CreateMap<CardAttachment, CardAttachmentDto>();
    }
}