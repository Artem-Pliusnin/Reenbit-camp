using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class SubscriptionPlansRepository : 
    BaseRepository<SubscriptionPlan, int>, 
    ISubscriptionPlansRepository
{
    public SubscriptionPlansRepository(TrelloAppDbContext context) 
        : base(context)
    {}

    public async Task<List<SubscriptionPlan>> GetAllPlansAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.OrderBy(sp => sp.Id).ToListAsync(cancellationToken);
    }

    public async Task<SubscriptionPlan?> GetBySubscriptionName(
        string name, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(
            sp => sp.Name == name, 
            cancellationToken);
    }
}