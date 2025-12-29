namespace Domain.Requests.Invitations;

public sealed record CreateInvitationRequest(
    int BoardId,
    int InvitedUserId);