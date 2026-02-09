namespace Presentation.API.Contracts.Boards;

public sealed record GetArchivedBoardsQueryParameters(
    string? Title, 
    int Page = 1, 
    int PageSize = 12);