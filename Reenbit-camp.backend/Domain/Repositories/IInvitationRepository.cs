using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IInvitationRepository : IRepository<Invitation, int>
{
    Task<List<Invitation>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}