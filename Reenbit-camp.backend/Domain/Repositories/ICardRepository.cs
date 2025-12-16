using Domain.Entities;

namespace Domain.Repositories;

public interface ICardRepository : IRepository<Card, int>
{
    Task<List<Card>> GetByListIdAsync(int listId, CancellationToken cancellationToken = default);
    
    Task<Card?> GetLastListsCard(int listId, CancellationToken cancellationToken = default);
    
    Task MoveCardAsync(int cardId, int newListId, int newPosition, CancellationToken cancellationToken = default);
}