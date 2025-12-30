using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class LabelRepository : 
    BaseRepository<Label, int>, 
    ILabelRepository
{
    public LabelRepository(TrelloAppDbContext context) 
        : base(context)
    {}
    
    public async Task<List<Label>> GetByBoardIdAsync(
        int boardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(l => l.BoardId == boardId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Label>> GetNotСonnectedToCardAsync(
        int cardId,
        int boardId,
        CancellationToken cancellationToken = default)
    {
        
        return await _dbSet
            .Where(l => l.BoardId == boardId)
            .Where(l => !l.CardLabels.Any(cl => cl.CardId == cardId))
            .ToListAsync(cancellationToken);
    }
}