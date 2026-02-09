namespace Domain.Models;

public sealed record ArchivedBoardsFilter(
    string? Title,
    int Page, 
    int PageSize);