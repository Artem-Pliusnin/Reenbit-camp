using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.BoardMembers.Queries.HasRequiredRole;

public sealed record HasRequiredRoleQuery(
    int BoardId, 
    int UserId, 
    BoardRole MinimumRole)
    : IQuery<bool>;