using Application.Abstractions.Messaging;
using Domain.DTOs.CardAttachments;

namespace Application.CardAttachments.Queries.GetCardAttachments;

public sealed record GetCardAttachmentsQuery(int CardId) : IQuery<List<CardAttachmentDto>>;