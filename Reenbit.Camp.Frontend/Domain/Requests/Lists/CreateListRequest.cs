namespace Domain.Requests.Lists;

public sealed record CreateListRequest(
    int BoardId, 
    string Title);