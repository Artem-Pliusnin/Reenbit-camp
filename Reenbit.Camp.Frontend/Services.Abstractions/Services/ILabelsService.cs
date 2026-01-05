using Domain.Models.Labels;
using Domain.Requests.Labels;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ILabelsService
{
    Task<Result<List<LabelModel>>> GetByBoardAsync(int boardId);
    
    Task<Result<List<LabelModel>>> GetNotConnectedAsync(int cardId);
    
    Task<Result<LabelModel>> CreateAsync(CreateLabelRequest request);
    
    Task<Result<object>> UpdateAsync(int id, UpdateLabelRequest request);
    
    Task<Result<object>> DeleteAsync(int id);
}