using Domain.Entities;
using Domain.Repositories;
using Persistence.Database;

namespace Persistence.Repositories;

public class SessionRepository 
    : BaseRepository<Session, int>, ISessionRepository
{
    public SessionRepository(TrelloAppDbContext context)
    : base(context)
    {}
}