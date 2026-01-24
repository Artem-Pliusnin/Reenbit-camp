using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Errors;
using Domain.Shared;

namespace Application.FAQChat.Queries.AskQuestion;

public class AskQuestionQueryHandler : IQueryHandler<AskQuestionQuery, string>
{
    private readonly IChatService _chatService;

    public AskQuestionQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<Result<string>> Handle(AskQuestionQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _chatService.AskAsync(request.Question);
            
            return response;
        }
        catch (Exception e)
        {
            return Result.Failure<string>(FAQChatErrors.ResponseError);
        }
    }
}