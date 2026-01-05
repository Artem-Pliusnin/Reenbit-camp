using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CardMembersRepository : 
    BaseRepository<CardMember, int>,
    ICardMembersRepository
{
    public CardMembersRepository(DbContext context)
        : base(context)
    {}

    public async Task<List<CardMember>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(cm => cm.CardId == cardId)
            .Include(cm => cm.User)
            .ToListAsync(cancellationToken);
    }
}