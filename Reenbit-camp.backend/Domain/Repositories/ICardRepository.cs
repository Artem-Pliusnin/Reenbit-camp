using Domain.Entities;

namespace Domain.Repositories;

public interface ICardRepository : IRepository<Card, int>
{
    Task<Card?> GetLastListsCard(int listId, CancellationToken cancellationToken = default);
}