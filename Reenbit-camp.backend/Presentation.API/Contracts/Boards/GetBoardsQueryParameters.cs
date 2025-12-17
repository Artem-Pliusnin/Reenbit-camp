namespace Presentation.API.Contracts.Boards;

public sealed record GetBoardsQueryParameters(
    string? Title, 
    bool OnlyMyBoards, 
    int Page = 1, 
    int PageSize = 12);