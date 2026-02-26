namespace Presentation.API.Contracts.Cards;

public sealed record GetCardsQueryParameters(
    string? Title,
    int BoardId,
    bool OnlyAssignedToUser,
    List<int>? Labels,
    int Page = 1, 
    int PageSize = 10);