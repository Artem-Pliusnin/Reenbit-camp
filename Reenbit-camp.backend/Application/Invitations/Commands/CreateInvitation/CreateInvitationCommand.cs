using Application.Abstractions.Messaging;
using Domain.DTOs.Invitations;

namespace Application.Invitations.Commands.CreateInvitation;

public sealed record CreateInvitationCommand(
    int BoardId,
    int InvitedUserId,
    int InvitedByUserId
    ) : ICommand<InvitationDto>;