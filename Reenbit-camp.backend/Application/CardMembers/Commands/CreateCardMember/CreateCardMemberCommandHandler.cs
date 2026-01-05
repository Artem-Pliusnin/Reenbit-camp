using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.CardMembers;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardMembers.Commands.CreateCardMember;

internal class CreateCardMemberCommandHandler : ICommandHandler<CreateCardMemberCommand, CardMemberDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCardMemberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<CardMemberDto>> Handle(
        CreateCardMemberCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
            var card = await cardRepository
                .GetByIdWithListAsync(request.CardId, cancellationToken);

            if (card == null)
            {
                return Result.Failure<CardMemberDto>(CardErrors.CardDoesNotExistError);
            }
            
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var user = await userRepository
                .GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Result.Failure<CardMemberDto>(UserErrors.UserDoesNotExist);
            }

            if (!user.Boards.Any(b => b.BoardId == card.List.BoardId))
            {
                return Result.Failure<CardMemberDto>(CardMemberErrors.DoesNotBelongToBoardError);
            }
            
            var cardMembersRepository = _unitOfWork.GetRepository<ICardMembersRepository>();

            var cardMember = new CardMember()
            {
                CardId = request.CardId,
                User = user,
            };
            
            cardMembersRepository.Add(cardMember);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var response = _mapper.Map<CardMemberDto>(cardMember);
            
            return Result.Success(response);
        }
        catch
        {
            return Result.Failure<CardMemberDto>(CardMemberErrors.CreatCardMemberError);
        }
    }
}