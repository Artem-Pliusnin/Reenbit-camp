using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Constants.ArchivationConstants;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Commands.ArchivePendingBoards;

internal class ArchivePendingBoardsCommandHandler 
    : ICommandHandler<ArchivePendingBoardsCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueueService _messageQueueService;

    public ArchivePendingBoardsCommandHandler(
        IUnitOfWork unitOfWork, 
        IMessageQueueService messageQueueService)
    {
        _unitOfWork = unitOfWork;
        _messageQueueService = messageQueueService;
    }
    
    public async Task<Result<int>> Handle(
        ArchivePendingBoardsCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var boardRepository = _unitOfWork.GetRepository<IBoardRepository>();

            var pendingBoards = await boardRepository
                .GetPendingBoardIdsAsync(cancellationToken);

            if (!pendingBoards.Any())
            {
                return Result.Success(0);
            }

            foreach (var board in pendingBoards)
            {
                await _messageQueueService.SendMessageAsync(
                    board.Id,
                    ArchivationConstants.ArchivationQueUerName,
                    cancellationToken);
            }
            
            return pendingBoards.Count;
        }
        catch
        {
            return Result.Failure<int>(BoardErrors.ArchiveBoardError);
        }
    }
}