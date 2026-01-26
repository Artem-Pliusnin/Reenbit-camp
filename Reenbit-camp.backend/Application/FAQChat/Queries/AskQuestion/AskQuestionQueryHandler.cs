using System.Text.Json;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Errors;
using Domain.Shared;

namespace Application.FAQChat.Queries.AskQuestion;

internal class AskQuestionQueryHandler : IQueryHandler<AskQuestionQuery, string>
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
            var response = await _chatService
                .AskAsync(request.UserId, request.Question);
            
            return response;
        }
        catch (Exception e)
        {
            return Result.Failure<string>(FAQChatErrors.ResponseError);
        }
    }
}