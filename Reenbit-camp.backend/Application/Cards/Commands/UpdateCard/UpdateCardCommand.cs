using Application.Abstractions.Messaging;

namespace Application.Cards.Commands.UpdateCard;

public sealed record UpdateCardCommand(
    int UserId,
    int CardId,
    string Title,
    string? Description) 
    : ICommand;