using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository : IRepository<User, int>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}