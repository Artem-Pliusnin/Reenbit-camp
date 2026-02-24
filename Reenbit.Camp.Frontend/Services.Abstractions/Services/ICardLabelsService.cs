using Domain.Models.Labels;
using Domain.Requests.Labels;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ICardLabelsService
{
    Task<Result<List<CardLabelModel>>> GetByCardAsync(int boardId, int cardId);
    
    Task<Result<CardLabelModel>> CreateAsync(int boardId, CreateCardLabelRequest request);
    
    Task<Result<object>> DeleteAsync(int boardId, int id);
}
