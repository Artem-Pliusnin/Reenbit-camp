using Domain.Shared;

namespace Domain.Errors;

public static class LabelErrors
{
    public static readonly Error CreateLabelError = new(
        "Label.CreateError",
        "Error occured while creating the label.");
    
    public static readonly Error UpdateLabelError = new(
        "Label.UpdateError",
        "Error occured while updating the label.");
    
    public static readonly Error LabelDoesNotExistError = new(
        "Label.NotExistListError",
        "Label with given id does not exist.");
}