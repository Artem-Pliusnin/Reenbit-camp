using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class CardLabelsRepository: 
    BaseRepository<CardLabel, int>, 
    ICardLabelsRepository
{
    public CardLabelsRepository(TrelloAppDbContext context) 
        : base(context)
    {}

    public async Task<List<CardLabel>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(cl => cl.CardId == cardId)
            .Include(cl => cl.Label)
            .ToListAsync(cancellationToken);
    }
}