using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class LabelRepository : 
    BaseRepository<Label, int>, 
    ILabelRepository
{
    public LabelRepository(TrelloAppDbContext context) 
        : base(context)
    {}
    
    public async Task<List<Label>> GetByBoardIdAsync(
        int boardId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(label => label.BoardId == boardId)
            .ToListAsync(cancellationToken);
    }
}