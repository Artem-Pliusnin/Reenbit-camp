using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class UserRepository : BaseRepository<User, int>, IUserRepository
{
    public UserRepository(TrelloAppDbContext context)
        : base(context)
    {}
    
    public async Task<User?> GetByIdWithAvatarAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Avatar)
            .Include(u => u.Subscription)
                .ThenInclude(us => us.SubscriptionPlan)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
    
    public async Task<bool> ExistsByEmailAsync(string email,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<List<User>> GetForInvitationAsync(
        int boardId, 
        int userId, 
        string? query, 
        int limit = 5,
        CancellationToken cancellationToken = default)
    {
        var usersQuery = _dbSet
            .Where(u => u.Id != userId)
            .Where(u => !u.Boards.Any(b => b.BoardId == boardId))
            .Where(u => !u.ReceivedInvitations.Any(i => 
                i.BoardId == boardId && 
                i.Status == InvitationStatus.Pending));

        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<User>();
        }
        
        var search = query.Trim().ToLowerInvariant();
        usersQuery = usersQuery.Where(u => 
            (u.FirstName + " " + u.LastName).ToLower().Contains(search) 
            || u.Email.ToLower().Contains(search));
        
        return await usersQuery
            .OrderBy(u => (u.FirstName + " " + u.LastName))
            .Include(u => u.Avatar)
            .Include(u => u.Subscription)
                .ThenInclude(us => us.SubscriptionPlan)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetNotСonnectedToCardAsync(
        int cardId, 
        int boardId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Boards.Any(bm => bm.BoardId == boardId))
            .Where(u => !u.Cards.Any(cm => cm.CardId == cardId))
            .Include(u => u.Avatar)
            .Include(u => u.Subscription)
                .ThenInclude(us => us.SubscriptionPlan)
            .ToListAsync(cancellationToken);
    }
}