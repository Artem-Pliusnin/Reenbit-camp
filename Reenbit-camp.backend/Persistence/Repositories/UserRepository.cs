using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class UserRepository : BaseRepository<User, int>, IUserRepository
{
    public UserRepository(TrelloAppDbContext context)
        : base(context)
    {}


    public async Task<bool> ExistsByEmailAsync(string email,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }
}