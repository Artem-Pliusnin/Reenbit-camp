using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.CardAttachments;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardAttachments.Queries.GetCardAttachments;

internal class GetCardAttachmentsQueryHandler : IQueryHandler<GetCardAttachmentsQuery, List<CardAttachmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCardAttachmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
     
    public async Task<Result<List<CardAttachmentDto>>> Handle(
        GetCardAttachmentsQuery request, 
        CancellationToken cancellationToken)
    {
        var cardAttachmentRepository = _unitOfWork.GetRepository<ICardAttachmentRepository>();
        
        var attachments = await cardAttachmentRepository
            .GetByCardIdAsync(request.CardId, cancellationToken);

        var response = _mapper.Map<List<CardAttachmentDto>>(attachments);
        
        return response;
    }
}