using Domain.DTOs.Shared;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class BoardRepository :
    BaseRepository<Board, int>,
    IBoardRepository
{
    public BoardRepository(TrelloAppDbContext context)
        : base(context)
    {
    }

    public async Task<PaginationDto<Board>> GetByUserIdAsync(
        int userId,
        BoardsFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(b => b.Members)
            .Where(b => b.Members.Any(m => m.UserId == userId));

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            var search = filter.Title.ToLowerInvariant();
            query = query.Where(b => b.Title.ToLower().Contains(search));
        }

        if (filter.OnlyMyBoards)
        {
            query = query.Where(b => b.CreatedBy == userId);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var currentPage = filter.Page < 1 ? 1 : filter.Page;
        
        if (totalPages == 0)
        {
            currentPage = 1;
        }
        else if (currentPage > totalPages)
        {
            currentPage = totalPages;
        }

        var boards = await query
            .OrderByDescending(b => b.CreationDate)
            .Skip((currentPage - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginationDto<Board>
        {
            Dtos = boards,
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
    }

    public async Task<Board?> GetFullInfoAsync(
        int boardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .Include(b => b.Lists.OrderBy(l => l.Position))
                .ThenInclude(l => l.Cards.OrderBy(c => c.Position))
                    .ThenInclude(c => c.Labels)
                        .ThenInclude(cl => cl.Label)
            .Include(b => b.Lists.OrderBy(l => l.Position))
                .ThenInclude(l => l.Cards.OrderBy(c => c.Position))
                    .ThenInclude(c => c.Members)
                        .ThenInclude(cm => cm.User)
            .FirstOrDefaultAsync(b => b.Id == boardId, cancellationToken);
    }
}
