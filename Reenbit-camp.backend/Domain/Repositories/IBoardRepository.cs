using Domain.DTOs.Shared;
using Domain.Entities;
using Domain.Models;

namespace Domain.Repositories;

public interface IBoardRepository : IRepository<Board, int>
{
    Task<PaginationDto<Board>> GetByUserIdAsync(
        int userId, 
        BoardsFilter filter,
        CancellationToken cancellationToken = default);
    
    Task<PaginationDto<Board>> GetArchivedByUserIdAsync(
        int userId, 
        ArchivedBoardsFilter filter,
        CancellationToken cancellationToken = default);
    
    Task<Board?> GetFullInfoAsync(
        int boardId, 
        CancellationToken cancellationToken = default);
    
    Task<List<Board>> GetPendingBoardIdsAsync(
        CancellationToken cancellationToken = default);
    
    Task<int> CountActiveUserOwnedBoardsAsync(
        int userId,
        CancellationToken cancellationToken = default);
    
    Task<List<Board>> GetUserBoardsForStatusChangeAsync(
        int userId,
        CancellationToken cancellationToken = default);
    
}