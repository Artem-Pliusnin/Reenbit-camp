using Domain.Entities;
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
        return await _dbSet.FirstOrDefaultAsync(
            us => us.UserId == userId, 
            cancellationToken); 
    }
}