namespace Presentation.API.Contracts.Boards;

public sealed record GetBoardsQueryParameters(string? Title, bool OnlyMyBoards);