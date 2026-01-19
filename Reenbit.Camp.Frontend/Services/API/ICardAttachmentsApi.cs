using Domain.Requests.Labels;
using Domain.Responses.CardAttachments;
using Refit;

namespace Services.API;

public interface ICardAttachmentsApi
{
    [Get("/CardAttachments/card/{id}")]
    Task<ApiResponse<List<CardAttachmentDto>>> GetByCardAsync(int id);

    [Multipart]
    [Post("/CardAttachments")]
    Task<ApiResponse<CardAttachmentDto>> CreateAsync(
        [AliasAs("cardId")] int cardId,
        [AliasAs("file")] StreamPart file);
    
    [Delete("/CardAttachments/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int id);
}