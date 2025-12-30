using Application.Abstractions.Messaging;
using Domain.DTOs.Users;

namespace Application.Users.Queries.GetInviteSuggestionUsers;

public sealed record GetInviteSuggestionUsersQuery(
    int BoardId, 
    int UserId, 
    string? Query,
    int Limit = 5) 
    : IQuery<List<UserDto>>;