using Domain.Responses.Labels;

namespace Domain.Responses.Cards;

public sealed record CardDto(
    int Id, 
    string Title, 
    int Position,
    bool IsCompleted,
    DateTime? StartDate, 
    DateTime? DueDate,
    List<CardLabelDto> Labels);