using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Constants.ArchivationConstants;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Commands.RestoreBoard;

public class RestoreBoardCommandHandler : ICommandHandler<RestoreBoardCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IArchivationLogsService _archivationLogsService;
    private readonly IMessageQueueService _messageQueueService;

    public RestoreBoardCommandHandler(
        IUnitOfWork unitOfWork, 
        IArchivationLogsService archivationLogsService, 
        IMessageQueueService messageQueueService)
    {
        _unitOfWork = unitOfWork;
        _archivationLogsService = archivationLogsService;
        _messageQueueService = messageQueueService;
    }
    public async Task<Result> Handle(RestoreBoardCommand request, CancellationToken cancellationToken)
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

            if (board.Status == BoardStatus.Active 
                || board.Status == BoardStatus.Restoring)
            {
                return Result.Failure(BoardErrors.NotArchivedError);
            }

            if (board.Status == BoardStatus.Pending)
            {
                board.Status = BoardStatus.Active;
                
                await _archivationLogsService
                    .SaveArchivationLogAsync(board.Id, ArchiveStatus.Restored);
            }
            else
            {
                await _messageQueueService.SendMessageAsync(
                    board.Id,
                    ArchivationConstants.RestorationQueueName,
                    cancellationToken);
            
                board.Status = BoardStatus.Restoring;
                
                await _archivationLogsService
                    .SaveArchivationLogAsync(board.Id, ArchiveStatus.SentToRestorationServiceBusQueue);
            }
            
            boardRepository.Update(board);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(BoardErrors.ArchiveBoardError);
        }
    }
}