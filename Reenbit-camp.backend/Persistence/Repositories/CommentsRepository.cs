using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class CommentsRepository :
    BaseRepository<Comment, int>,
    ICommentsRepository
{
    public CommentsRepository(TrelloAppDbContext context) 
        : base(context)
    {
    }

    public async Task<List<Comment>> GetByCardIdAsync(
        int cardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(c => c.CardId == cardId)
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}