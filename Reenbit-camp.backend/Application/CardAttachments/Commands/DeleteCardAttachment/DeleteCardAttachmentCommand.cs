using Application.Abstractions.Messaging;

namespace Application.CardAttachments.Commands.DeleteCardAttachment;

public sealed record DeleteCardAttachmentCommand(int CardAttachmentId) : ICommand;