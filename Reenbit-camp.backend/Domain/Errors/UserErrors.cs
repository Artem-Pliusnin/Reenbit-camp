using Domain.Shared;

namespace Domain.Errors;

public static class UserErrors
{
    public static readonly Error EmailAlreadyTaken = new(
        "User.EmailAlreadyTaken",
        "The specified email is already taken");
    
    public static readonly Error NotFoundByEmail = new(
        "Users.NotFoundByEmail",
        "The user with the specified email was not found");

}