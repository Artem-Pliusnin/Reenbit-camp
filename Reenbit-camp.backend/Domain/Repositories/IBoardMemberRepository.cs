using Domain.Entities;

namespace Domain.Repositories;

public interface IBoardMemberRepository : IRepository<BoardMember, int>
{
    Task<List<BoardMember>> GetByBoardIdAsync(
        int boardId, 
        CancellationToken cancellationToken = default);
    
    Task<BoardMember?> GetByUserAndBoardIdAsync(
        int userId, 
        int boardId, 
        CancellationToken cancellationToken = default);
    
    Task<bool> IsUserMemberOfTheBoardAsync(
        int userId, 
        int boardId, 
        CancellationToken cancellationToken = default);
}