using Application.Abstractions.Messaging;
using Domain.DTOs.Invitations;

namespace Application.Invitations.Queries.GetUserInvitations;

public sealed record GetUserInvitationsQuery(int UserId) : IQuery<List<InvitationDto>>;