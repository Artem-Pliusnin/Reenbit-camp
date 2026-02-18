using Domain.Entities;

namespace Domain.Repositories;

public interface IUserSubscriptionsRepository : IRepository<UserSubscription, int>
{
    Task<UserSubscription?> GetUserSubscription(
        int userId, 
        CancellationToken cancellationToken = default);
    
    Task<UserSubscription?> GetBySubscriptionId(
        string SubscriptionId, 
        CancellationToken cancellationToken = default);
}