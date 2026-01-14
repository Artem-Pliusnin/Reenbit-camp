using Application.Abstractions.Messaging;
using Domain.DTOs.Invitations;
using Domain.Entities;

namespace Application.Invitations.Commands.DeclinedInvitation;

public sealed record DeclineInvitationCommand(
    int UserId, 
    int InvitationId) 
    : ICommand<InvitationDto>;