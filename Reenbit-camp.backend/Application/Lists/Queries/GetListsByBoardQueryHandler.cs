using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Lists;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Lists.Queries;

public class GetListsByBoardQueryHandler : IQueryHandler<GetListsByBoardQuery, List<ListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetListsByBoardQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<ListDto>>> Handle(
        GetListsByBoardQuery request, 
        CancellationToken cancellationToken)
    {
        var listRepository = _unitOfWork.GetRepository<IListRepository>();
        
        var lists = await listRepository
            .GetByBoardIdAsync(request.BoardId, cancellationToken);

        var response = _mapper.Map<List<ListDto>>(lists);
        
        return response;
    }
}