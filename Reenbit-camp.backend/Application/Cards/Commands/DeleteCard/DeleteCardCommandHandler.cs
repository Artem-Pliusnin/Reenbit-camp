using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Commands.DeleteCard;

internal class DeleteCardCommandHandler : ICommandHandler<DeleteCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public DeleteCardCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }
    
    public async Task<Result> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var card = await cardRepository
            .GetByIdWithAttachmentsAsync(request.CardId, cancellationToken);

        if (card == null)
        {
            return Result.Failure<bool>(CardErrors.CardDoesNotExistError);
        }
        
        await cardRepository.DeleteCardAsync(
            card.Id, 
            card.ListId, 
            cancellationToken);

        foreach (var attachment in card.Attachments)
        {
            await _fileService.DeleteAttachmentFileAsync(attachment.FileName, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}