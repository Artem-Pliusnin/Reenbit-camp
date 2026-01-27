using Domain.Enums;
using Domain.Models.FAQChat;
using Domain.Requests.FAQChat;
using Domain.Shared;
using Refit;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class FAQChatService : IFAQChatService
{
    private readonly IFAQChatApi _faqChatApi;

    public FAQChatService(IFAQChatApi faqChatApi)
    {
        _faqChatApi = faqChatApi;
    }
    
    public async Task<Result<MessageModel>> AskQuestionAsync(AskQuestionRequest request)
    {
        var response = await _faqChatApi.AskQuestionAsync(request);

        return response.HandleResultWithMapping(content 
            => new MessageModel
            {
                Message = content,
                Sender = SenderType.Bot
            });
    }

    public async Task<Result> StartSessionAsync()
    {
        var response = await _faqChatApi.StartSessionAsync();

        return response.HandleResult();
    }

    public async Task<Result> EndSessionAsync()
    {
        var response = await _faqChatApi.EndSessionAsync();

        return response.HandleResult();
    }

    public async Task<Result> ImportFileAsync(StreamPart file)
    {
        var response = await _faqChatApi.ImportFileAsync(file);

        return response.HandleResult();
    }
}