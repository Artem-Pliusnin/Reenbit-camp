using Application.Abstractions.Messaging;

namespace Application.Invitations.Commands.DeleteInvitation;

public sealed record DeleteInvitationCommand(int InvitationId) : ICommand;