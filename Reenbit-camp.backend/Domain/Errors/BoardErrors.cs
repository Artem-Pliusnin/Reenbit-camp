using Domain.Shared;

namespace Domain.Errors;

public static class BoardErrors
{
    public static readonly Error UpdateBoardError = new(
        "Board.UpdateError",
        "Error occured while updating the board.");
    
    public static readonly Error ArchiveBoardError = new(
        "Board.ArchiveBoard",
        "Error occured while archiving the board.");
    
    public static readonly Error AlreadyArchivedError = new(
        "Board.AlreadyArchived",
        "Board already archived.");
    
    public static readonly Error NotArchivedError = new(
        "Board.NotArchived",
        "Board is not archived.");
    
    public static readonly Error CreateBoardError = new(
        "Board.CreateError",
        "Error occured while creating the board.");
    
    public static readonly Error BoardDoesNotExistError = new(
        "Board.NotExistBoardError",
        "Board with given id does not exist.");
}