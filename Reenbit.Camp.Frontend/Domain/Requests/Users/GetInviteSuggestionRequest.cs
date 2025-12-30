namespace Domain.Requests.Users;

public sealed record GetInviteSuggestionRequest(
    int BoardId, 
    string? Query,
    int Limit);