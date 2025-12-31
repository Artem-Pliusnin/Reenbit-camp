using Domain.Shared;

namespace Domain.Errors;

public static class CardLabelErrors
{
    public static readonly Error CreatCardLabelError = new(
        "CardLabel.CreateError",
        "Error occured while adding label to the card.");
    
    public static readonly Error DoesNotBelongToBoardError = new(
        "CardLabel.MismatchError",
        "Label does not belong to this board.");
    
    public static readonly Error CardLabelDoesNotExistError = new(
        "CardLabel.NotExistListError",
        "Card label with given id does not exist.");
}