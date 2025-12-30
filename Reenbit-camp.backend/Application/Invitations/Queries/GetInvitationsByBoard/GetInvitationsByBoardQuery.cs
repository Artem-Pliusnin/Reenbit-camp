using Application.Abstractions.Messaging;
using Domain.DTOs.Invitations;

namespace Application.Invitations.Queries.GetInvitationsByBoard;

public record GetInvitationsByBoardQuery(int BoardId) : IQuery<List<InvitationDto>>;