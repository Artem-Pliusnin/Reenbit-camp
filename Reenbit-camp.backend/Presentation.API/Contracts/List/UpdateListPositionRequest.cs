namespace Presentation.API.Contracts.List;

public sealed record UpdateListPositionRequest(
    int ListId, 
    int NewPosition);