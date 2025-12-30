using Application.Abstractions.Messaging;

namespace Application.Invitations.Commands.DeclinedInvitation;

public sealed record DeclineInvitationCommand(
    int UserId, 
    int InvitationId) 
    : ICommand;