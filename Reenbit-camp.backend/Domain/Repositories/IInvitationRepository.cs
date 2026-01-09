using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IInvitationRepository : IRepository<Invitation, int>
{
    Task<List<Invitation>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    
    Task<List<Invitation>> GetBoardPendingAsync(int boardId, CancellationToken cancellationToken = default);
    
    Task<Invitation?> GetByIdWithIncludesAsync(int invitationId, CancellationToken cancellationToken = default);
}