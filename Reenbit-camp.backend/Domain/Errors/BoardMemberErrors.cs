using Domain.Shared;

namespace Domain.Errors;

public static class BoardMemberErrors
{
    public static readonly Error UpdateBoardMemberError = new(
        "BoardMember.UpdateError",
        "Error occured while updating the board member.");
    
    public static readonly Error BoardMemberDoesNotExistError = new(
        "BoardMember.NotExistError",
        "Board member with given id does not exist.");
}