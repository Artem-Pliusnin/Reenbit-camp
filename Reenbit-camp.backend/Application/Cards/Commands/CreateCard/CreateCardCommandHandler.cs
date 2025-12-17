using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Cards;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Commands.CreateCard;

internal class CreateCardCommandHandler : ICommandHandler<CreateCardCommand, CardDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    
    public async Task<Result<CardDto>> Handle(
        CreateCardCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var cardRepository = _unitOfWork.GetRepository<ICardRepository>();

            var card = new Card()
            {
                ListId = request.ListId,
                Title = request.Title,
                Position = await GetNewCardPositionAsync(request.ListId),
                LastUpdatedBy = request.UserId,
                LastUpdateDate = DateTime.UtcNow
            };
            
            cardRepository.Add(card);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CardDto>(card);

            return response;
        }
        catch
        {
            return Result.Failure<CardDto>(CardErrors.CreateCardError);
        }
    }
    
    private async Task<int> GetNewCardPositionAsync(int listId)
    {
        var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var lastCard = await cardRepository.GetLastListsCard(listId);
        
        return lastCard != null ? lastCard.Position + 1 : 1;
    }
}