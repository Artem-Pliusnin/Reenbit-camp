using Domain.Models.Cards;
using Domain.Requests.Cards;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ICardsService
{
    Task<Result<CardModel>> CreateAsync(CreateCardRequest request);
    
    Task<Result<List<CardModel>>> GetByListAsync(int listId);
    
    Task<Result<CardInfoModel>> GetInfoAsync(int id);
    
    Task<Result<object>> UpdateAsync(int id, UpdateCardRequest request);
    
    Task<Result<object>> UpdateStatusAsync(int id, UpdateCardStatusRequest request);
    
    Task<Result<object>> UpdatePositionAsync(int id, UpdateCardPositionRequest request);
    
    Task<Result<object>> UpdateDeadlineAsync(int id, UpdateCardDeadlineRequest request);
    
    Task<Result<object>> DeleteAsync(int id);
}