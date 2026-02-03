using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Commands.ArchiveBoard;

internal class ArchiveBoardCommandHandler : ICommandHandler<ArchiveBoardCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveBoardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
            
            board.Status = BoardStatus.Pending;

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