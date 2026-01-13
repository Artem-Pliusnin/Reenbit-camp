using Application.Abstractions.Messaging;
using Domain.DTOs.BoardMembers;

namespace Application.Invitations.Commands.AcceptInvitation;

public sealed record AcceptInvitationCommand(
    int UserId, 
    int InvitationId) 
    : ICommand<BoardMemberDto>;