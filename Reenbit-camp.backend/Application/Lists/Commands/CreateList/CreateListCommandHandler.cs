using Application.Abstractions.Messaging;
using Domain.DTOs.Lists;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Lists.Commands.CreateList;

public class CreateListCommandHandler : ICommandHandler<CreateListCommand, ListDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateListCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
                Position = list.Position
            };

            return response;
        }
        catch (Exception ex)
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