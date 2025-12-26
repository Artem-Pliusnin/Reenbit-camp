using Application.Abstractions.Messaging;

namespace Application.Invitations.Commands.DeclinedInvitation;

public record DeclinedInvitationCommand(
    int UserId, 
    int InvitationId) 
    : ICommand;