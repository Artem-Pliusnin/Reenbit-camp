using Domain.Requests.Labels;
using Domain.Responses.CardAttachments;
using Refit;

namespace Services.API;

public interface ICardAttachmentsApi
{
    [Get("/Board/{boardId}/CardAttachments/card/{id}")]
    Task<ApiResponse<List<CardAttachmentDto>>> GetByCardAsync(int boardId, int id);

    [Multipart]
    [Post("/Board/{boardId}/CardAttachments")]
    Task<ApiResponse<CardAttachmentDto>> CreateAsync(
        int boardId,
        [AliasAs("cardId")] int cardId,
        [AliasAs("file")] StreamPart file);
    
    [Delete("/Board/{boardId}/CardAttachments/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int boardId, int id);
}