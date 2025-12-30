namespace Presentation.API.Contracts.Invitations;

public sealed record CreateInvitationRequest(
    int BoardId,
    int InvitedUserId);