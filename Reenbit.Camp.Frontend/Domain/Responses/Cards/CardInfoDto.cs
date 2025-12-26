namespace Domain.Responses.Cards;

public sealed record CardInfoDto(
    int Id, 
    string Title, 
    string? Description, 
    bool IsCompleted, 
    DateTime? StartDate, 
    DateTime? DueDate);