using Domain.Responses.Cards;

namespace Domain.Responses.Lists;

public sealed record ListDto(
    int Id, 
    string Title, 
    int Position, 
    List<CardDto> Cards
);