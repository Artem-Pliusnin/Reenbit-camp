using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Errors;
using Domain.Shared;

namespace Application.FAQChat.Commands.EndChatSession;

internal class EndChatSessionCommandHandler : ICommandHandler<EndChatSessionCommand>
{
    private readonly IChatService _chatService;

    public EndChatSessionCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<Result> Handle(EndChatSessionCommand request, CancellationToken cancellationToken)
    {
        var result = _chatService.EndChatSession(request.UserId);

        if (result)
        {
            return Result.Success();
        }

        return Result.Failure(FAQChatErrors.EndSessionError);
    }
}