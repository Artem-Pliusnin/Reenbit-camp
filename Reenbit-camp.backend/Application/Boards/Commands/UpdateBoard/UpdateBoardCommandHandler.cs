using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Commands.UpdateBoard;

internal class UpdateBoardCommandHandler : ICommandHandler<UpdateBoardCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBoardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        UpdateBoardCommand request,
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

            board.Title = request.Title;
            board.LastUpdatedBy = request.UserId;
            board.LastUpdateDate = DateTime.UtcNow;

            boardRepository.Update(board);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(BoardErrors.UpdateBoardError);
        }
    }
}