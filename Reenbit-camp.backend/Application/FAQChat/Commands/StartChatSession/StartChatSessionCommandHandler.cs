using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Shared;

namespace Application.FAQChat.Commands.StartChatSession;

internal class StartChatSessionCommandHandler : ICommandHandler<StartChatSessionCommand>
{
    private readonly IChatService _chatService;

    public StartChatSessionCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<Result> Handle(StartChatSessionCommand request, CancellationToken cancellationToken)
    {
        _chatService.StartChatSession(request.UserId);
        
        return Result.Success();
    }
}