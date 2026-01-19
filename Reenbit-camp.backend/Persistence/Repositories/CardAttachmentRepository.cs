using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class CardAttachmentRepository : BaseRepository<CardAttachment, int>, ICardAttachmentRepository
{
    public CardAttachmentRepository(TrelloAppDbContext context) 
        : base(context)
    {}

    public async Task<List<CardAttachment>> GetByCardIdAsync(
        int cardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(ca => ca.CardId == cardId)
            .OrderByDescending(ca => ca.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}