namespace Domain.Requests.Lists;

public sealed record UpdateListRequest(
    int ListId, 
    string Title);