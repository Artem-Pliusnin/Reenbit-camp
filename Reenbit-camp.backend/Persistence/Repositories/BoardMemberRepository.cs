using Domain.Entities;
using Domain.Enums;
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

    public async Task<BoardMember?> GetByIdWithBoardAsync(
        int boardMemberId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(bm => bm.Board)
            .FirstOrDefaultAsync(bm => bm.Id == boardMemberId, cancellationToken);
    }

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
                .ThenInclude(u => u.Avatar)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<bool> IsUserMemberOfTheBoardAsync(
        int userId, 
        int boardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(
            bm => bm.UserId == userId && bm.BoardId == boardId,
            cancellationToken);
    }

    public async Task<BoardMember?> GetNewOwnerAsync(
        int boardId, 
        BoardMember removedOwner, 
        CancellationToken cancellationToken = default)
    {
        var remainingMembers = await _dbSet
            .Where(m => m.BoardId == boardId && m.Id != removedOwner.Id)
            .OrderBy(m => m.Role)
            .ThenBy(m => m.Id)
            .ToListAsync();
        
        if (!remainingMembers.Any())
        {
            return null;
        }
    
        return remainingMembers.First();
    }
}