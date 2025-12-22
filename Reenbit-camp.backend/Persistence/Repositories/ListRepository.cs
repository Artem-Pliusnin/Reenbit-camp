using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class ListRepository : 
    BaseRepository<List, int>, 
    IListRepository
{
    public ListRepository(TrelloAppDbContext context) 
        : base(context)
    {}

    public async Task<List<List>> GetByBoardIdAsync(
        int boardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(list => list.BoardId == boardId)
            .OrderBy(l => l.Position)
            .Include(l => l.Cards)
            .ToListAsync(cancellationToken);
    }

    public async Task<List?> GetLastBoardList(
        int boardId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(list => list.BoardId == boardId)
            .OrderByDescending(list => list.Position)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task MoveListAsync(
        int boardId, 
        int listId, 
        int newPosition, 
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL move_list_in_board({0}, {1}, {2})",
            boardId,
            listId,
            newPosition
        );
    }
}