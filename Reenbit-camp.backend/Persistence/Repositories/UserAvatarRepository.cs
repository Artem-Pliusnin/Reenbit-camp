using Domain.Entities;
using Domain.Repositories;
using Persistence.Database;

namespace Persistence.Repositories;

public class UserAvatarRepository : BaseRepository<UserAvatar, int>, IUserAvatarRepository
{
    public UserAvatarRepository(TrelloAppDbContext context) : 
        base(context)
    {}
}