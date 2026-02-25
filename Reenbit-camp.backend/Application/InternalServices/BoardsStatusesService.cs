using Application.Abstractions.Services;
using Domain.Enums;
using Domain.Repositories;

namespace Application.InternalServices;

public class BoardsStatusesService : IBoardsStatusesService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public BoardsStatusesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task UpdateUserBoardsStatuses(int userId, int userLimit)
    {
        var boardsRepository = _unitOfWork
            .GetRepository<IBoardRepository>();
        
        var boards = await boardsRepository
            .GetUserBoardsForStatusChangeAsync(userId);

        for (var i = 0; i < boards.Count; i++)
        {
            boards[i].Status = i < userLimit
                    ? BoardStatus.Active
                    : BoardStatus.Blocked;
        }

        await _unitOfWork.SaveChangesAsync();
    }
}