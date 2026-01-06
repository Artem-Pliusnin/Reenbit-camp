using AutoMapper;
using Domain.Models.Comments;
using Domain.Requests.Comments;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class CommentsService : ICommentsService
{
    private readonly ICommentsApi _commentsApi;
    private readonly IMapper _mapper;

    public CommentsService(ICommentsApi commentsApi, IMapper mapper)
    {
        _commentsApi = commentsApi;
        _mapper = mapper;
    }
    
    public async Task<Result<List<CommentModel>>> GetByCardAsync(int cardId)
    {
        var response = await _commentsApi.GetByCardAsync(cardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<CommentModel>>(content));
    }

    public async Task<Result<CommentModel>> CreateAsync(CreateCommentRequest request)
    {
        var response = await _commentsApi.CreateAsync(request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<CommentModel>(content));
    }

    public async Task<Result<object>> UpdateAsync(int id, UpdateCommentRequest request)
    {
        var response = await _commentsApi.UpdateAsync(request, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> DeleteAsync(int id)
    {
        var response = await _commentsApi.DeleteAsync(id);

        return response.HandleResult();
    }
}