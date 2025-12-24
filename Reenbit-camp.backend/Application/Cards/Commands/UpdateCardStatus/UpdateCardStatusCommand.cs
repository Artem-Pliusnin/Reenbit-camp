using Application.Abstractions.Messaging;

namespace Application.Cards.Commands.UpdateCardStatus;

public sealed record UpdateCardStatusCommand(
    int CardId, 
    bool IsCompleted, 
    int UserId) : ICommand;