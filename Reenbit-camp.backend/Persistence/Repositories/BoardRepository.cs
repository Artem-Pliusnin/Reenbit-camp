using Domain.Entities;
using Domain.Models;
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

    public async Task<List<Board>> GetByUserIdAsync(
        int userId,
        BoardsFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Include(b => b.Members)
            .Where(b => b.Members.Any(m => m.UserId == userId));
        
        if (!string.IsNullOrEmpty(filter.Title))
        {
            var search = filter.Title.ToLowerInvariant();
            query = query.Where(b => b.Title.ToLower().Contains(search));
        }

        if (filter.OnlyMyBoards)
        {
            query = query.Where(b => b.CreatedBy == userId);
        }
        
        return await query.ToListAsync(cancellationToken);
    }
}