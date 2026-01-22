using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using AutoMapper;
using Domain.Constants.FIleConstants;
using Domain.DTOs.CardAttachments;
using Domain.Entities;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardAttachments.Commands.CreateCardAttachment;

internal class CreateCardAttachmentCommandHandler 
    : ICommandHandler<CreateCardAttachmentCommand, CardAttachmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public CreateCardAttachmentCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }
    
    public async Task<Result<CardAttachmentDto>> Handle(
        CreateCardAttachmentCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var cardAttachmentRepository = _unitOfWork.GetRepository<ICardAttachmentRepository>();

            var fileDto = await _fileService
                .UploadFileAsync(
                    request.FileContent,
                    request.FileName,
                    request.ContentType,
                    FileDirectoriesConstants.Attachments,
                    cancellationToken);

            var cardAttachment = new CardAttachment()
            {
                CardId = request.CardId,
                FileName = fileDto.FileName,
                FileUrl = fileDto.FileUrl,
                ContentType = request.ContentType
            };

            cardAttachmentRepository.Add(cardAttachment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CardAttachmentDto>(cardAttachment);

            return Result.Success(response);
        }
        catch (FileStorageException ex)
        {
            return Result.Failure<CardAttachmentDto>(CardAttachmentErrors.SavingFileError);
        }
        catch
        {
            return Result.Failure<CardAttachmentDto>(CardAttachmentErrors.CreateCardAttachmentError);
        }
    }
}