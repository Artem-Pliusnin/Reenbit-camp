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
            .ToListAsync(cancellationToken);
    }
}