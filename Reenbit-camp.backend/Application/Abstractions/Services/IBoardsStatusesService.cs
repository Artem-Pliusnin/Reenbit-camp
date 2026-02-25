namespace Application.Abstractions.Services;

public interface IBoardsStatusesService
{
    Task UpdateUserBoardsStatuses(int userId, int userLimit);
}