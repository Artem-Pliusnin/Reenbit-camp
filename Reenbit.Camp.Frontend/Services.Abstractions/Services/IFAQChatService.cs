using Domain.Models.FAQChat;
using Domain.Requests.FAQChat;
using Domain.Shared;
using Refit;

namespace Services.Abstractions.Services;

public interface IFAQChatService
{
    Task<Result<MessageModel>> AskQuestionAsync(AskQuestionRequest request);
    
    Task<Result> StartSessionAsync();
    
    Task<Result> EndSessionAsync();
    
    Task<Result> ImportFileAsync(StreamPart file);
}