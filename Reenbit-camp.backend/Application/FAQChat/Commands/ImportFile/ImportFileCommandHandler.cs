using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Errors;
using Domain.Shared;

namespace Application.FAQChat.Commands.ImportFile;

public class ImportFileCommandHandler : ICommandHandler<ImportFileCommand>
{
    private readonly IChatService _chatService;

    public ImportFileCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<Result> Handle(ImportFileCommand request, CancellationToken cancellationToken)
    {
        var fileId = Guid.NewGuid();
        
        var result = await _chatService
           .ImportAsync(request.Content, fileId.ToString() ,request.FileName);

        Console.WriteLine(fileId);

        if (result)
        {
           return Result.Success();
        } 
        
        return Result.Failure(FAQChatErrors.FileImportError);
    }
}