using Domain.Models.CardAttachments;
using Domain.Shared;
using Refit;

namespace Services.Abstractions.Services;

public interface ICardAttachmentService
{
    Task<Result<List<CardAttachmentModel>>> GetByCardAsync(int cardId);
    
    Task<Result<CardAttachmentModel>> CreateAsync(int cardId, StreamPart file);
    
    Task<Result<object>> DeleteAsync(int id);
}
