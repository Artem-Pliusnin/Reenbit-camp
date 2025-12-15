namespace Presentation.API.Contracts.List;

public sealed record CreateListRequest(
    int BoardId, 
    string Title);