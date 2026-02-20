using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class UserSubscriptionsRepository : 
    BaseRepository<UserSubscription, int>,
    IUserSubscriptionsRepository
{
    public UserSubscriptionsRepository(TrelloAppDbContext context) 
        : base(context)
    {}

    public async Task<UserSubscription?> GetUserSubscription(
        int userId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(us => us.SubscriptionPlan)
            .FirstOrDefaultAsync(us => us.UserId == userId, cancellationToken); 
    }

    public async Task<UserSubscription?> GetBySubscriptionId(
        string SubscriptionId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(
            us => us.StripeSubscriptionId == SubscriptionId, 
            cancellationToken); 
    }

    public async Task<List<UserSubscription>> GetExpiredSubscriptionsAsync(
        DateTime now, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(s =>
                s.SubscriptionPlan.Name != "Free" 
                && ((s.SubscriptionStatus == SubscriptionStatus.Canceled &&
                     s.CurrentPeriodEnd.HasValue &&
                     s.CurrentPeriodEnd <= now)
                    ||
                    (s.SubscriptionStatus == SubscriptionStatus.Unpaid &&
                     s.CurrentPeriodEnd.HasValue &&
                     s.CurrentPeriodEnd <= now)
                    ||
                    (s.CancelAtPeriodEnd &&
                     s.CurrentPeriodEnd.HasValue &&
                     s.CurrentPeriodEnd.Value <= now))
            )
            .ToListAsync(cancellationToken);
    }
}