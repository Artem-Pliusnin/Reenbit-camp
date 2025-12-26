using Domain.Shared;

namespace Domain.Errors;

public class InvitationErrors
{
    public static readonly Error CreateInvitationError = new(
        "Invitation.CreateError",
        "Error occured while creating the invitation.");
    
    public static readonly Error InvitationDoesNotExist = new(
        "Invitation.DoesNotExist",
        "Invitation does not exist.");
    
    public static readonly Error TheSameUserError = new(
        "Invitation.TheSameUserError",
        "Invited user and inviting user can't be the same.");
    
    public static readonly Error UpdateInvitationError = new(
        "Invitation.UpdateError",
        "Error occured while updating the invitation.");
    
    public static readonly Error InvitationDoesNotBelongsToUserError = new(
        "Invitation.DoesNotBelongsToUserError",
        "Invitation does not belong to this user.");
}