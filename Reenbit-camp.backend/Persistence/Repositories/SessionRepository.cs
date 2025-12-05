using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class SessionRepository 
    : BaseRepository<Session, int>, ISessionRepository
{
    public SessionRepository(TrelloAppDbContext context)
    : base(context)
    {}

    public async Task<Session?> GetSessionByRefreshToken(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Token == refreshToken, cancellationToken);
    }
}