using Domain.Entities;

namespace Domain.Repositories;

public interface IBoardMemberRepository : IRepository<BoardMember, int>
{
    Task<List<BoardMember>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
}