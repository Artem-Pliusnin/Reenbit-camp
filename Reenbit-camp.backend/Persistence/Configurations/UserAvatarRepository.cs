using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Repositories;

namespace Persistence.Configurations;

public class UserAvatarRepository : BaseRepository<UserAvatar, int>, IUserAvatarRepository
{
    public UserAvatarRepository(TrelloAppDbContext context) : 
        base(context)
    {}
}