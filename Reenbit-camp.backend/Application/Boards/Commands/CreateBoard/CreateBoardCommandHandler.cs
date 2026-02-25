using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Commands.CreateBoard;

internal class CreateBoardCommandHandler : ICommandHandler<CreateBoardCommand, BoardCardDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBoardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BoardCardDto>> Handle(
        CreateBoardCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var boardRepository = _unitOfWork.GetRepository<IBoardRepository>();
            var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();
            var userSubscriptionRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();
            
            var userBoardsCount = await boardRepository
                .CountActiveUserOwnedBoardsAsync(request.UserId, cancellationToken);
            
            var userSubscription = await userSubscriptionRepository
                .GetUserSubscription(request.UserId, cancellationToken);

            if (userSubscription == null)
            {
                return Result.Failure<BoardCardDto>(SubscriptionErrors.UserDataNotFound);
            }

            if (userBoardsCount >= userSubscription.SubscriptionPlan.BoardsLimit)
            {
                return Result.Failure<BoardCardDto>(SubscriptionErrors.BoardsLimitReached);
            }

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

            var response = new BoardCardDto()
            {
                Id = board.Id,
                Title = board.Title,
                Status = board.Status,
                OwnerId = request.UserId
            };

            return response;

        }
        catch
        {
            return Result.Failure<BoardCardDto>(BoardErrors.CreateBoardError);
        }
    }
}