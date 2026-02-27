namespace Domain.Models;

public sealed record CardsFilterModel(
    string? Title,
    int BoardId,
    bool OnlyAssignedToUser,
    List<int>? Labels,
    int Page, 
    int PageSize);