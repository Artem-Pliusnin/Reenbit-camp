using Domain.Entities;

namespace Domain.Repositories;

public interface ICardMembersRepository: IRepository<CardMember, int>
{
    Task<List<CardMember>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
}