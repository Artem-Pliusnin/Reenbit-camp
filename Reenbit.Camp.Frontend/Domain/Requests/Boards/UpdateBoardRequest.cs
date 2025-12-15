namespace Domain.Requests.Boards;

public sealed record UpdateBoardRequest(int BoardId, string Title);