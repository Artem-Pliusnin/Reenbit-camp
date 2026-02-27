namespace Domain.Requests.Lists;

public sealed record UpdateListPositionRequest(
    int ListId, 
    int NewPosition);