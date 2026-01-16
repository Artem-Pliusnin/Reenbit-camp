using Application.Abstractions.Messaging;
using Domain.DTOs.CardAttachments;
using Domain.Entities;

namespace Application.CardAttachments.Commands.CreateCardAttachment;

public sealed record CreateCardAttachmentCommand(
    int CardId,
    Stream FileContent,
    string FileName,
    string ContentType) : ICommand<CardAttachmentDto> ;