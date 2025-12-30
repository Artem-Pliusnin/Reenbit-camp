using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class BoardMemberRepository :
    BaseRepository<BoardMember, int>,
    IBoardMemberRepository
{
    public BoardMemberRepository(TrelloAppDbContext context) 
        : base(context)
    {}

    public async Task<List<BoardMember>> GetByBoardIdAsync(
        int boardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(bm => bm.BoardId == boardId)
            .Include(bm => bm.Board)
            .Include(bm => bm.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<BoardMember?> GetByUserAndBoardIdAsync(int userId, int boardId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(bm => bm.BoardId == boardId && bm.UserId == userId)
            .Include(bm => bm.Board)
            .Include(bm => bm.User)
            .FirstOrDefaultAsync(cancellationToken);
    }
}