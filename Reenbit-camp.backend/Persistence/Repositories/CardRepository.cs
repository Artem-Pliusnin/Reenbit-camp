using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class CardRepository :
    BaseRepository<Card, int>,
    ICardRepository
{
    public CardRepository(TrelloAppDbContext context) 
        : base(context)
    {}

    public async Task<Card?> GetLastListsCard(int listId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(card => card.ListId == listId)
            .OrderByDescending(card => card.Position)
            .FirstOrDefaultAsync(cancellationToken);
    }
}