using Domain.Entities;
using Domain.Repositories;
using Persistence.Database;

namespace Persistence.Repositories;

public class UserRepository : BaseRepository<User, int>, IUserRepository
{
    public UserRepository(TrelloAppDbContext context)
    : base(context)
    {}
}