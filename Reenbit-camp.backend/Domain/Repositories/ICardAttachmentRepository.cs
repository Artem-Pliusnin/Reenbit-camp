using Domain.Entities;

namespace Domain.Repositories;

public interface ICardAttachmentRepository : IRepository<CardAttachment, int>
{
    Task<List<CardAttachment>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
}