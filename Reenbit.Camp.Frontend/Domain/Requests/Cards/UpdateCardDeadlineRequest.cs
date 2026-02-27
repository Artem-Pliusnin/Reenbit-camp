namespace Domain.Requests.Cards;

public sealed record UpdateCardDeadlineRequest(
    int CardId,
    DateTime? StartDate, 
    DateTime? DueDate);