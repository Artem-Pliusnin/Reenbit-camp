using Domain.Entities;

namespace Domain.Repositories;

public interface ISessionRepository : IRepository<Session, int>
{
    Task<Session?> GetSessionByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    
    Task<Session?> GetSessionByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}