using Domain.Entities;
using Domain.Repositories;
using Persistence.Database;

namespace Persistence.Repositories;

public class BoardMemberRepository :
    BaseRepository<BoardMember, int>,
    IBoardMemberRepository
{
    public BoardMemberRepository(TrelloAppDbContext context) 
        : base(context)
    {}
    
}