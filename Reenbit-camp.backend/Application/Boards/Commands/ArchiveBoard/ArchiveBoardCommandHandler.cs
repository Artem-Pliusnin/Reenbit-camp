using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Commands.ArchiveBoard;

internal class ArchiveBoardCommandHandler : ICommandHandler<ArchiveBoardCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IArchivationLogsService _archivationLogsService;

    public ArchiveBoardCommandHandler(IUnitOfWork unitOfWork, 
        IArchivationLogsService archivationLogsService)
    {
        _unitOfWork = unitOfWork;
        _archivationLogsService = archivationLogsService;
    }
    
    public async Task<Result> Handle(
        ArchiveBoardCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var boardRepository = _unitOfWork.GetRepository<IBoardRepository>();

            var board = await boardRepository
                .GetByIdAsync(request.BoardId, cancellationToken);

            if (board == null)
            {
                return Result.Failure(BoardErrors.BoardDoesNotExistError);
            }

            if (board.Status != BoardStatus.Active)
            {
                return Result.Failure(BoardErrors.AlreadyArchivedError);
            }
            
            board.Status = BoardStatus.Pending;

            boardRepository.Update(board);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            await _archivationLogsService
                .SaveArchivationLogAsync(board.Id, ArchiveStatus.MarkedAsPending);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(BoardErrors.ArchiveBoardError);
        }
    }
}