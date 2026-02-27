using AutoMapper;
using Domain.Models.CardAttachments;
using Domain.Responses.CardAttachments;

namespace Services.Mapping;

public class CardAttachmentProfile: Profile
{
    public CardAttachmentProfile()
    {
        CreateMap<CardAttachmentDto, CardAttachmentModel>().ReverseMap();
    }
}