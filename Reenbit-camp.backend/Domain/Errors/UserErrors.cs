using Domain.Shared;

namespace Domain.Errors;

public static class UserErrors
{
    public static readonly Error EmailAlreadyTaken = new(
        "User.EmailAlreadyTaken",
        "The specified email is already taken");
    
    public static readonly Error InvalidCredentials = new(
        "Users.InvalidCredentials",
        "Invalid credentials");

    public static readonly Error UserUnauthorized = new(
        "Users.Unauthorized",
        "User is not authorized");
}