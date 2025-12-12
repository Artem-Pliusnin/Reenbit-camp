using Domain.Shared;

namespace Domain.Errors;

public static class ListErrors
{
    public static readonly Error CreateListError = new(
        "List.CreateError",
        "Error occured while creating the list.");
}