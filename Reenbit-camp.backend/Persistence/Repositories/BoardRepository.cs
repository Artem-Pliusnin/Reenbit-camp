using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class BoardRepository : 
    BaseRepository<Board, int>, 
    IBoardRepository
{
    public BoardRepository(TrelloAppDbContext context) 
        : base(context)
    {}

    public async Task<List<Board>> GetByUserIdAsync(int userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(b => b.Members)
            .Where(b => b.Members.Any(m => m.UserId == userId))
            .ToListAsync(cancellationToken);
    }
}