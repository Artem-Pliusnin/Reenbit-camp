using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Cards;
using Domain.DTOs.Lists;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Lists.Commands.CreateList;

internal class CreateListCommandHandler : ICommandHandler<CreateListCommand, ListDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateListCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<ListDto>> Handle(
        CreateListCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var listRepository = _unitOfWork.GetRepository<IListRepository>();

            var list = new List()
            {
                BoardId = request.BoardId,
                Title = request.Title,
                Position = await GetNewListPositionAsync(request.BoardId),
                LastUpdatedBy = request.UserId,
                LastUpdateDate = DateTime.UtcNow
            };
            
            listRepository.Add(list);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new ListDto()
            {
                Id = list.Id,
                Title = list.Title,
                Position = list.Position,
                Cards = _mapper.Map<List<CardDto>>(list.Cards),
            };

            return response;
        }
        catch
        {
            return Result.Failure<ListDto>(ListErrors.CreateListError);
        }
    }

    private async Task<int> GetNewListPositionAsync(int boardId)
    {
        var listRepository = _unitOfWork.GetRepository<IListRepository>();
        
        var lastBoardList = await listRepository.GetLastBoardList(boardId);
        
        return lastBoardList != null ? lastBoardList.Position + 1 : 1;
    }
}