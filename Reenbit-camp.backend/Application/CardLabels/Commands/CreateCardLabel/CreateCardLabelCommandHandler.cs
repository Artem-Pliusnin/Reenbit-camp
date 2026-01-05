using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.CardLabels;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardLabels.Commands.CreateCardLabel;

internal class CreateCardLabelCommandHandler : ICommandHandler<CreateCardLabelCommand, CardLabelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCardLabelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<CardLabelDto>> Handle(
        CreateCardLabelCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
            var card = await cardRepository
                .GetByIdWithListAsync(request.CardId, cancellationToken);

            if (card == null)
            {
                return Result.Failure<CardLabelDto>(CardErrors.CardDoesNotExistError);
            }
            
            var labelRepository = _unitOfWork.GetRepository<ILabelRepository>();
            var label = await labelRepository
                .GetByIdAsync(request.LabelId, cancellationToken);

            if (label == null)
            {
                return Result.Failure<CardLabelDto>(LabelErrors.LabelDoesNotExistError);
            }

            if (card.List.BoardId != label.BoardId)
            {
                return Result.Failure<CardLabelDto>(CardLabelErrors.DoesNotBelongToBoardError);
            }
            
            var cardLabelsRepository = _unitOfWork.GetRepository<ICardLabelsRepository>();

            var cardLabel = new CardLabel()
            {
                CardId = request.CardId,
                Label = label,
            };
            
            cardLabelsRepository.Add(cardLabel);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var response = _mapper.Map<CardLabelDto>(cardLabel);
            
            return Result.Success(response);
        }
        catch
        {
            return Result.Failure<CardLabelDto>(CardLabelErrors.CreatCardLabelError);
        }
    }
}