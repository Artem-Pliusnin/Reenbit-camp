using Application.Abstractions.Messaging;

namespace Application.Invitations.Commands.AcceptInvitation;

public sealed record AcceptInvitationCommand(
    int UserId, 
    int InvitationId) 
    : ICommand;