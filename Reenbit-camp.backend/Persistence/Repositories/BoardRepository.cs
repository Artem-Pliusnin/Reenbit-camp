using Domain.Entities;
using Domain.Repositories;
using Persistence.Database;

namespace Persistence.Repositories;

public class BoardRepository : 
    BaseRepository<Board, int>, 
    IBoardRepository
{
    public BoardRepository(TrelloAppDbContext context) 
        : base(context)
    {}
}