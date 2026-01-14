using Application.Abstractions.Messaging;
using Domain.DTOs.Invitations;

namespace Application.Invitations.Commands.DeleteInvitation;

public sealed record DeleteInvitationCommand(int InvitationId) : ICommand<InvitationDto>;