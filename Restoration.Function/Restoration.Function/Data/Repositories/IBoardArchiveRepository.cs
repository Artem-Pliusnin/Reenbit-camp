using Restoration.Function.Models.Etities;

namespace Restoration.Function.Data.Repositories;

public interface IBoardArchiveRepository
{
    Task RestoreBoardData(Board board);
    
    Task MarkBoardAsActive(int boardId);
}