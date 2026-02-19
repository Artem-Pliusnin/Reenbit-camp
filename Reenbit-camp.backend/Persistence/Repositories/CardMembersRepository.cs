using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class CardMembersRepository : 
    BaseRepository<CardMember, int>,
    ICardMembersRepository
{
    public CardMembersRepository(TrelloAppDbContext context)
        : base(context)
    {}

    public async Task<List<CardMember>> GetByCardIdAsync(
        int cardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(cm => cm.CardId == cardId)
            .Include(cm => cm.User)
                .ThenInclude(u =>u.Avatar)
            .Include(cm => cm.User)
                .ThenInclude(u =>u.Subscription)
                    .ThenInclude(us => us.SubscriptionPlan)
            .ToListAsync(cancellationToken);
    }
    
    public async Task DeleteAllByUserAndBoard(
        int userId,
        int boardId,
        CancellationToken cancellationToken = default)
    {
        var cardMembers = await _dbSet
            .Include(cm => cm.Card)
                .ThenInclude(c => c.List)
            .Where(cm => cm.UserId == userId && cm.Card.List.BoardId == boardId)
            .ToListAsync(cancellationToken);
        
        _dbSet.RemoveRange(cardMembers);
    }
}