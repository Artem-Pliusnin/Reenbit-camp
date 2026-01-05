using Domain.Shared;

namespace Domain.Errors;

public class CardMemberErrors
{
    public static readonly Error CreatCardMemberError = new(
        "CardMember.CreateError",
        "Error occured while adding member to the card.");
    
    public static readonly Error DoesNotBelongToBoardError = new(
        "CardMember.MismatchError",
        "Member does not belong to this board.");
    
    public static readonly Error CardMemberDoesNotExistError = new(
        "CardMember.NotExistListError",
        "Card member with given id does not exist.");
}