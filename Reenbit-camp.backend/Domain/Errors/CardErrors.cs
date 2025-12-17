using Domain.Shared;

namespace Domain.Errors;

public static class CardErrors
{
    public static readonly Error CreateCardError = new(
        "Card.CreateError",
        "Error occured while creating the card.");
    
    public static readonly Error UpdateCardError = new(
        "Card.UpdateError",
        "Error occured while updating the card.");
    
    public static readonly Error CardDoesNotExistError = new(
        "Card.NotExistListError",
        "Card with given id does not exist.");
}