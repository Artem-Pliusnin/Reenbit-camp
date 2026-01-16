using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.CardLabels.Commands.DeleteCardLabel;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardAttachments.Commands.DeleteCardAttachment;

internal class DeleteCardAttachmentCommandHandler : ICommandHandler<DeleteCardAttachmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService; 

    public DeleteCardAttachmentCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }
    
    public async Task<Result> Handle(
        DeleteCardAttachmentCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var cardAttachmentRepository = _unitOfWork.GetRepository<ICardAttachmentRepository>();

            var cardAttachment = await cardAttachmentRepository
                .GetByIdAsync(request.CardAttachmentId, cancellationToken);

            if (cardAttachment == null)
            {
                return Result.Failure(CardLabelErrors.CardLabelDoesNotExistError);
            }
            
            cardAttachmentRepository.Remove(cardAttachment);
            
            await _fileService.DeleteAttachmentFileAsync(cardAttachment.FileName, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(true);
        }
        catch
        {
            return Result.Failure(CardAttachmentErrors.DeleteCardAttachmentError);
        }
    }
}