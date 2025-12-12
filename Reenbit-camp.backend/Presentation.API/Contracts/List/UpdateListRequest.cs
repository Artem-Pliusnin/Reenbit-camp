namespace Presentation.API.Contracts.List;

public sealed record UpdateListRequest(
    int ListId, 
    string Title);