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
    
    public async Task<Card?> GetByIdWithListAsync(
        int cardId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.List)
            .FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken);
    }

    public async Task<List<Card>> GetByListIdAsync(
        int listId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(card => card.ListId == listId)
            .OrderBy(c => c.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<Card?> GetLastListsCard(
        int listId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(card => card.ListId == listId)
            .OrderByDescending(card => card.Position)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task MoveCardAsync(
        int cardId, 
        int newListId, 
        int newPosition, 
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL move_card({0}, {1}, {2})",
            cardId,
            newListId,
            newPosition
        );
    }

    public async Task DeleteCardAsync(
        int cardId, 
        int listId, 
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            "CALL delete_card_and_reorder({0}, {1})",
            cardId,
            listId
        );
    }
}