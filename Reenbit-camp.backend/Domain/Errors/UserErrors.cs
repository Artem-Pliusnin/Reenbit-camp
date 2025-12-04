using Domain.Shared;

namespace Domain.Errors;

public static class UserErrors
{
    public static readonly Error EmailAlreadyTaken = new(
        "User.EmailAlreadyTaken",
        "The specified email is already taken");
}