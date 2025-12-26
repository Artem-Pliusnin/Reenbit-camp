namespace Domain.Models.Cards;

public sealed record UpdateCardModel(
    string Title,
    bool IsCompleted,
    DateTime? StartDate,
    DateTime? DueDate);