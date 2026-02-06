using System.Text.Json;
using Restoration.Function.Models.Enums;
using Restoration.Function.Models.Etities;
using Restoration.Function.ServicesAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Restoration.Function.Data.Repositories;

public class BoardArchiveRepository : IBoardArchiveRepository
{
    private readonly BoardsDbContext _context;
    private readonly IArchivationLogsService _archivationLogsService;
    private readonly ILogger<BoardArchiveRepository> _logger;

    public BoardArchiveRepository(
        BoardsDbContext context, 
        ILogger<BoardArchiveRepository> logger, 
        IArchivationLogsService archivationLogsService)
    {
        _context = context;
        _logger = logger;
        _archivationLogsService = archivationLogsService;
    }
    
    public async Task RestoreBoardData(Board board)
    {
        var existingBoard = await _context.Boards
            .FirstOrDefaultAsync(b => b.Id == board.Id);

        if (existingBoard is null)
        {
            await _archivationLogsService.SaveArchivationLogAsync(
                board.Id, 
                ArchiveStatus.FailedToSaveToDataBase);
        }

        if (existingBoard.Status != BoardStatus.Archived)
        {
            return;
        }
        
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Labels.AddRangeAsync(board.Labels);
            await _context.BoardMembers.AddRangeAsync(board.Members);
            await _context.Lists.AddRangeAsync(board.Lists);
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            await _archivationLogsService
                .SaveArchivationLogAsync(board.Id, ArchiveStatus.SavedToDataBase);
            
            _logger.LogInformation($"Successfully restored all related data for BoardId {board.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            
            _logger.LogError(ex,
                $"Error restoring board related data for BoardId {board.Id}");
            
            await _archivationLogsService
                .SaveArchivationLogAsync(board.Id, ArchiveStatus.FailedToSaveToDataBase);
            throw;
        }
    }

    public async Task MarkBoardAsActive(int boardId)
    {
        var board = await _context.Boards
            .Where(b => b.Id == boardId)
            .FirstOrDefaultAsync();
        
        if (board != null)
        {
            board.Status = BoardStatus.Active;
            _context.Boards.Update(board);
            
            await _context.SaveChangesAsync();
        }
    }
}