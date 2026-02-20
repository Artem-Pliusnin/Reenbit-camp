using Domain.Entities;

namespace Domain.Repositories;

public interface ISubscriptionPlansRepository : IRepository<SubscriptionPlan, int>
{
    Task<List<SubscriptionPlan>> GetAllPlansAsync( CancellationToken cancellationToken = default);
    Task<SubscriptionPlan?> GetBySubscriptionName(string name, CancellationToken cancellationToken = default);
}