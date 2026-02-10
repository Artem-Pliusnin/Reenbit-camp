using Archivation.Function.Models.Etities;

namespace Archivation.Function.Data.Repositories;

public interface IBoardArchiveRepository
{
    Task<Board?> GetBoardArchiveDataAsync(int boardId);
    
    Task MarkBoardAsArchived(int boardId);
    
    Task DeleteBoardRelatedDataAsync(int boardId);
    
    Task RestoreBoardData(Board board);
    
    Task MarkBoardAsActive(int boardId);
}