using Domain.Entities;

namespace Domain.Repositories;

public interface ICardLabelsRepository : IRepository<CardLabel, int>
{
    Task<List<CardLabel>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
}