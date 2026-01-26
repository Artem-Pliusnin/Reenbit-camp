using Domain.Requests.FAQChat;
using Domain.Responses.CardAttachments;
using Refit;

namespace Services.API;

public interface IFAQChatApi
{
    [Post("/FAQChat/question")]
    Task<ApiResponse<string>> AskQuestionAsync(
        [Body] AskQuestionRequest request);

    [Multipart]
    [Post("/FAQChat/file/import")]
    Task<ApiResponse<object>> ImportFileAsync(
        [AliasAs("File")] StreamPart file);
    
    [Post("/FAQChat/session/start")]
    Task<ApiResponse<object>> StartSessionAsync();
    
    [Post("/FAQChat/session/end")]
    Task<ApiResponse<object>> EndSessionAsync();
}