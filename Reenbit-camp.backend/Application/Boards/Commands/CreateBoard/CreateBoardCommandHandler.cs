using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
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
        try
        {
            var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();

            var board = new Board()
            {
                Title = request.Title,
                CreatedBy = request.UserId,
                CreationDate = DateTime.UtcNow,
            };

            var boardMember = new BoardMember()
            {
                Board = board,
                UserId = request.UserId,
                Role = BoardRole.Owner
            };

            boardMemberRepository.Add(boardMember);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new BoardDto()
            {
                Id = board.Id,
                Title = board.Title,
            };

            return response;

        }
        catch
        {
            return Result.Failure<BoardDto>(BoardErrors.CreateBoardError);
        }
    }
}