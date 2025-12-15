using Domain.Entities;
using Domain.Models;

namespace Domain.Repositories;

public interface IBoardRepository : IRepository<Board, int>
{
    Task<List<Board>> GetByUserIdAsync(
        int userId, 
        BoardsFilter filter,
        CancellationToken cancellationToken = default);
}