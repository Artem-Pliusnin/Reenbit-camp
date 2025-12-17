using Application.Abstractions.Messaging;

namespace Application.Cards.Commands.UpdateCardDeadline;

public sealed record UpdateCardDeadlineCommand(
    int CardId,
    int UserId,
    DateTime? StartDate,
    DateTime? DueDate) 
    : ICommand;