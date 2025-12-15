using Domain.Shared;

namespace Domain.Errors;

public static class ListErrors
{
    public static readonly Error CreateListError = new(
        "List.CreateError",
        "Error occured while creating the list.");
    
    public static readonly Error UpdateListError = new(
        "List.UpdateError",
        "Error occured while updating the list.");
    
    public static readonly Error ListDoesNotExistError = new(
        "List.NotExistListError",
        "List with given id does not exist.");
}