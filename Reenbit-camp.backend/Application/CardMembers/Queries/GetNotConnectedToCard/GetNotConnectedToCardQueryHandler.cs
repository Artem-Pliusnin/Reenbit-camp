using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Users;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardMembers.Queries.GetNotConnectedToCard;

internal class GetNotConnectedToCardQueryHandler : IQueryHandler<GetNotConnectedToCardQuery, List<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetNotConnectedToCardQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<UserDto>>> Handle(GetNotConnectedToCardQuery request, CancellationToken cancellationToken)
    {
        var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var card = await cardRepository
            .GetByIdWithListAsync(request.CardId, cancellationToken);
        
        if (card == null)
        {
            return Result.Failure<List<UserDto>>(CardErrors.CardDoesNotExistError);
        }
        
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        
        var users = await userRepository
            .GetNotСonnectedToCardAsync(
                request.CardId, 
                card.List.BoardId, 
                cancellationToken);

        var response = _mapper.Map<List<UserDto>>(users);
        
        return response;
    }
}