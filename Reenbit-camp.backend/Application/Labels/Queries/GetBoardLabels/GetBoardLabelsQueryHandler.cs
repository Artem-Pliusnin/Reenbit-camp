using Application.Abstractions.Messaging;
using Application.Lists.Queries.GetListsByBoard;
using AutoMapper;
using Domain.DTOs.Labels;
using Domain.DTOs.Lists;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Labels.Queries.GetBoardLabels;

internal class GetBoardLabelsQueryHandler : IQueryHandler<GetBoardLabelsQuery, List<LabelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBoardLabelsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<LabelDto>>> Handle(
        GetBoardLabelsQuery request, 
        CancellationToken cancellationToken)
    {
        var labelRepository = _unitOfWork.GetRepository<ILabelRepository>();
        
        var labels = await labelRepository
            .GetByBoardIdAsync(request.BoardId, cancellationToken);

        var response = _mapper.Map<List<LabelDto>>(labels);
        
        return response;
    }
}