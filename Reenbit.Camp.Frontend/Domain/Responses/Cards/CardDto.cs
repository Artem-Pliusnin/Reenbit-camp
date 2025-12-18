namespace Domain.Responses.Cards;

public sealed record CardDto(
    int Id, 
    string Title, 
    int Position, 
    DateTime? StartDate, 
    DateTime? DueDate);