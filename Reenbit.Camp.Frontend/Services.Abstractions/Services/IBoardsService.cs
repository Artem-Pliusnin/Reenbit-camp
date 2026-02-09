using Domain.Models.Boards;
using Domain.Requests.Boards;
using Domain.Responses.Boards;
using Domain.Responses.Shared;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IBoardsService
{
    Task<Result<BoardModel>> CreateAsync(CreateBoardRequest request);
    
    Task<Result<BoardInfoModel>> GetInfoAsync(int boardId);
    
    Task<Result<PaginationDto<BoardModel>>> GetByUserAsync(BoardsFilterModel filter);
    
    Task<Result<object>> UpdateAsync(int id, UpdateBoardRequest request);
    
    Task<Result<PaginationDto<BoardModel>>> GetArchivedByUserAsync(ArchivedBoardsFilter filter);
    
    Task<Result<object>> ArchiveBoard(int id);
    
    Task<Result<object>> RestoreBoard(int id);
}