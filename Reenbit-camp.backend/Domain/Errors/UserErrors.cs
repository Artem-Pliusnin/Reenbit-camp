using Domain.Shared;

namespace Domain.Errors;

public static class UserErrors
{
    public static readonly Error EmailAlreadyTaken = new(
        "User.EmailAlreadyTaken",
        "The specified email is already taken");
    
    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "Invalid credentials");

    public static readonly Error UserUnauthorized = new(
        "User.Unauthorized",
        "User is not authorized");
    
    public static readonly Error UserDoesNotExistError = new(
        "User.DoesNotExistError",
        "User with given id does not exist.");
    
    public static readonly Error UserUpdateError = new(
        "User.UpdateError",
        "Error occured while updating user.");
}