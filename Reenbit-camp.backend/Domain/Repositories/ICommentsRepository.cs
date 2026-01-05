using Domain.Entities;

namespace Domain.Repositories;

public interface ICommentsRepository : IRepository<Comment, int>
{
    Task<List<Comment>> GetByCardIdAsync(
        int cardId, 
        CancellationToken cancellationToken = default);
}