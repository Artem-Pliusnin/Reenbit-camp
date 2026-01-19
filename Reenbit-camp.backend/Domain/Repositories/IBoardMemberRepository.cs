using Domain.Entities;

namespace Domain.Repositories;

public interface IBoardMemberRepository : IRepository<BoardMember, int>
{
    Task<BoardMember?> GetByIdWithBoardAsync(
        int boardMemberId, 
        CancellationToken cancellationToken = default);
    
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

    Task<BoardMember?> GetNewOwnerAsync(
        int boardId, 
        BoardMember removedOwner,
        CancellationToken cancellationToken = default);
}