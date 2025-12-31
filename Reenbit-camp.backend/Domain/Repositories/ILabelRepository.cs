using Domain.Entities;

namespace Domain.Repositories;

public interface ILabelRepository : IRepository<Label, int>
{
    Task<List<Label>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
    
    Task<List<Label>> GetNotСonnectedToCardAsync(
        int cardId, 
        int boardId, 
        CancellationToken cancellationToken = default);
}