using Domain.Entities;

namespace Domain.Repositories;

public interface ISubscriptionPlansRepository : IRepository<SubscriptionPlan, int>
{
    Task<SubscriptionPlan?> GetBySubscriptionName(string name, CancellationToken cancellationToken = default);
}