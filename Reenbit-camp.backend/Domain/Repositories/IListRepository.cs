using Domain.Entities;

namespace Domain.Repositories;

public interface IListRepository : IRepository<List, int>
{
    Task<List<List>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
    
    Task<List?> GetLastBoardList(int boardId, CancellationToken cancellationToken = default);

    Task MoveListAsync(int boardId, int listId, int newPosition, CancellationToken cancellationToken = default);
    
    Task DeleteListAsync(int listId, int boardId, CancellationToken cancellationToken = default);
}