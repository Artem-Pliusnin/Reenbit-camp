using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Commands.CreateBoard;

public class CreateBoardCommandHandler : ICommandHandler<CreateBoardCommand, BoardDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBoardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BoardDto>> Handle(
        CreateBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var boardRepository = _unitOfWork.GetRepository<IBoardRepository>();

        var board = new Board()
        {
            Title = request.Title,
            CreatedBy = request.UserId,
            CreationDate = DateTime.UtcNow,
        };
        
        boardRepository.Add(board);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new BoardDto()
        {
            Id = board.Id,
            Title = board.Title,
        };
        
        return response;
    }
}