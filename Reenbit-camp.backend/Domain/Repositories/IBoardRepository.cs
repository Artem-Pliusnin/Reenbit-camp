using Domain.Entities;

namespace Domain.Repositories;

public interface IBoardRepository : IRepository<Board, int>
{
    Task<List<Board>> GetByUserIdAsync(int userId ,CancellationToken cancellationToken = default);
}