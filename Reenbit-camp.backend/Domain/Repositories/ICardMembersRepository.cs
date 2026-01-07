using Domain.Entities;

namespace Domain.Repositories;

public interface ICardMembersRepository: IRepository<CardMember, int>
{
    Task<List<CardMember>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);

    Task DeleteAllByUserAndBoard(int userId, int boardId, CancellationToken cancellationToken = default);
}