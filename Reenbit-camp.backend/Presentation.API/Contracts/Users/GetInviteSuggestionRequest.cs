namespace Presentation.API.Contracts.Users;

public sealed record GetInviteSuggestionRequest(
    int BoardId, 
    string? Query,
    int Limit);
