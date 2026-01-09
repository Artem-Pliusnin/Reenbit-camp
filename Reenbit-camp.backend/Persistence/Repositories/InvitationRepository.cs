using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class InvitationRepository : 
    BaseRepository<Invitation, int>, 
    IInvitationRepository
{
    public InvitationRepository(TrelloAppDbContext context) 
        : base(context)
    {}
    
    

    public async Task<List<Invitation>> GetByUserIdAsync(
        int userId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(i => 
                i.InvitedUserId == userId 
                && i.Status == InvitationStatus.Pending)
            .Include(i => i.Board)
            .Include(i => i.InvitedUser)
            .Include(i => i.InvitedByUser)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Invitation>> GetBoardPendingAsync(
        int boardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(i => 
                i.BoardId == boardId 
                && i.Status == InvitationStatus.Pending)
            .Include(i => i.Board)
            .Include(i => i.InvitedUser)
            .Include(i => i.InvitedByUser)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Invitation?> GetByIdWithIncludesAsync(
        int invitationId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Board)
            .Include(i => i.InvitedUser)
            .Include(i => i.InvitedByUser)
            .FirstOrDefaultAsync(i => i.Id == invitationId, cancellationToken);
    }
}