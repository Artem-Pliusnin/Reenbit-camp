using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Comments;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Comments.Queries.GetCardComments;

public class GetCardCommentsQueryHandler : IQueryHandler<GetCardCommentsQuery, List<CommentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCardCommentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<CommentDto>>> Handle(
        GetCardCommentsQuery request, 
        CancellationToken cancellationToken)
    {
        var commentsRepository = _unitOfWork.GetRepository<ICommentsRepository>();
        
        var comments = await commentsRepository
            .GetByCardIdAsync(request.CardId, cancellationToken);

        var response = _mapper.Map<List<CommentDto>>(comments);
        
        return response;
    }
}