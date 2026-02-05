using Archivation.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Archivation.Function.Data.Repositories;

public class BoardArchiveRepository : IBoardArchiveRepository
{
    private readonly BoardsDbContext _context;
    private readonly ILogger<BoardArchiveRepository> _logger;

    public BoardArchiveRepository(
        BoardsDbContext context, ILogger<BoardArchiveRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Board?> GetBoardArchiveDataAsync(int boardId)
    {
        return await _context.Boards.AsNoTracking()
            .Include(b => b.Lists)
                .ThenInclude(l => l.Cards)
                    .ThenInclude(c => c.Labels)
            .Include(b => b.Lists)
                .ThenInclude(l => l.Cards)
                    .ThenInclude(c => c.Members)
            .Include(b => b.Lists)
                .ThenInclude(l => l.Cards)
                    .ThenInclude(c => c.Attachments)
            .Include(b => b.Lists)
                .ThenInclude(l => l.Cards)
                    .ThenInclude(c => c.Comments)
            .Include(b => b.Members)
            .Include(b => b.Invitations)
            .Include(b => b.Labels)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public async Task MarkBoardAsArchived(int boardId)
    {
        var board = await _context.Boards.Where(b => b.Id == boardId).FirstOrDefaultAsync();
        if (board != null)
        {
            board.Status = BoardStatus.Archived;
            _context.Boards.Update(board);
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteBoardRelatedDataAsync(int boardId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var deletedLists = await _context.Lists
                .Where(l => l.BoardId == boardId)
                .ExecuteDeleteAsync();

            var deletedLabels = await _context.Labels
                .Where(l => l.BoardId == boardId)
                .ExecuteDeleteAsync();

            var deletedBoardMembers = await _context.BoardMembers
                .Where(bm => bm.BoardId == boardId)
                .ExecuteDeleteAsync();

            var deletedInvitations = await _context.Invitations
                .Where(i => i.BoardId == boardId)
                .ExecuteDeleteAsync();

            await transaction.CommitAsync();

            _logger.LogInformation(
                "Successfully deleted all related data for BoardId {BoardId} " +
                "(Lists: {Lists}, Labels: {Labels}, Members: {Members}, Invitations: {Invitations})", 
                boardId, deletedLists, deletedLabels, deletedBoardMembers, deletedInvitations);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, 
                "Error deleting board related data for BoardId {BoardId}", 
                boardId);
            throw;
        }
    }
}