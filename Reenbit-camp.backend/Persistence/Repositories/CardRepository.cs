using Domain.DTOs.Shared;
using Domain.Entities;
using Domain.Models;
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
    
    public async Task<Card?> GetByIdWithAttachmentsAsync(
        int cardId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Attachments)
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

    public async Task<InfiniteScrollDto<Card>> GetFilteredAsync(
        int userId, 
        CardsFilterModel filter, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Where(c => c.List.BoardId == filter.BoardId);

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            var search = filter.Title.ToLowerInvariant();
            query = query.Where(c => c.Title.ToLower().Contains(search));
        }

        if (filter.OnlyAssignedToUser)
        {
            query = query.Where(c => 
                c.Members.Any(cm => cm.UserId == userId));
        }

        if (filter.Labels is not null && filter.Labels.Count > 0)
        {
            query = query.Where(c =>
                c.Labels.Count(l => filter.Labels.Contains(l.LabelId)) == filter.Labels.Count
            );
        }

        var currentPage = filter.Page < 1 ? 1 : filter.Page;

        var items = await query
            .OrderByDescending(c => c.Id)
            .Include(c => c.Members)
                .ThenInclude(cm => cm.User)
                    .ThenInclude(u => u.Avatar)
            .Include(c => c.Labels)
                .ThenInclude(l => l.Label)
            .Skip((currentPage - 1) * filter.PageSize)
            .Take(filter.PageSize + 1)
            .ToListAsync(cancellationToken);

        var hasMore = items.Count > filter.PageSize;

        if (hasMore)
        {
            items.RemoveAt(items.Count - 1);
        }

        return new InfiniteScrollDto<Card>
        {
            Dtos = items,
            HasMore = hasMore
        };
    }
}