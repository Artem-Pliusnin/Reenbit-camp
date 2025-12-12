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

    public async Task<List<List>> GetByBoardIdAsync(int boardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(list => list.BoardId == boardId)
            .OrderBy(l => l.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<List?> GetLastBoardList(int boardId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(list => list.BoardId == boardId)
            .OrderByDescending(list => list.Position)
            .FirstOrDefaultAsync(cancellationToken);
    }
}