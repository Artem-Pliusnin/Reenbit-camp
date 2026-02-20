using Application.Abstractions.Messaging;

namespace Application.Cards.Commands.UpdateCardPosition;

public sealed record UpdateCardPositionCommand(
    int CardId,
    int NewListId, 
    int NewPosition)
    : ICommand;