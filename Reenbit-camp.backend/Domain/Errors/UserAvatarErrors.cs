using Domain.Shared;

namespace Domain.Errors;

public class UserAvatarErrors
{
    public static readonly Error UpdateUserAvatarError = new(
        "UserAvatar.UpdateError",
        "Error occured while updating the user avatar.");
    
    public static readonly Error SavingFileError = new(
        "UserAvatar.SavingFileError",
        "Error occured while saving the file.");
}