namespace Presentation.API.Contracts.Boards;

public sealed record UpdateBoardRequest(int BoardId, string Title);