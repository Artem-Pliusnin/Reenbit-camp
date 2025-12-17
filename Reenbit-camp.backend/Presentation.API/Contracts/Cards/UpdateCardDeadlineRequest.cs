namespace Presentation.API.Contracts.Cards;

public sealed record UpdateCardDeadlineRequest(
    int CardId,
    DateTime? StartDate, 
    DateTime? DueDate);