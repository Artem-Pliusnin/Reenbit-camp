using Domain.Models.Boards;
using Domain.Requests.Boards;
using Domain.Responses.Boards;
using Domain.Responses.Shared;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IBoardsService
{
    Task<Result<BoardModel>> CreateAsync(CreateBoardRequest request);
    
    Task<Result<PaginationDto<BoardModel>>> GetByUserAsync(BoardsFilterModel filter);
    
    Task<Result<object>> UpdateAsync(int id, UpdateBoardRequest request);
}