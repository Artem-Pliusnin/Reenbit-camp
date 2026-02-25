using Domain.Models.Cards;
using Domain.Requests.Cards;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ICardsService
{
    Task<Result<CardModel>> CreateAsync(int boardId, CreateCardRequest request);
    
    Task<Result<List<CardModel>>> GetByListAsync(int boardId, int listId);
    
    Task<Result<CardInfoModel>> GetInfoAsync(int boardId, int id);
    
    Task<Result<object>> UpdateAsync(int boardId, int id, UpdateCardRequest request);
    
    Task<Result<object>> UpdateStatusAsync(int boardId, int id, UpdateCardStatusRequest request);
    
    Task<Result<object>> UpdatePositionAsync(int boardId, int id, UpdateCardPositionRequest request);
    
    Task<Result<object>> UpdateDeadlineAsync(int boardId, int id, UpdateCardDeadlineRequest request);
    
    Task<Result<object>> DeleteAsync(int boardId, int id);
}