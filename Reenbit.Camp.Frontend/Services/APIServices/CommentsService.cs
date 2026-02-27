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
    
    public async Task<Result<List<CommentModel>>> GetByCardAsync(int boardId, int cardId)
    {
        var response = await _commentsApi.GetByCardAsync(boardId, cardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<CommentModel>>(content));
    }

    public async Task<Result<CommentModel>> CreateAsync(int boardId, CreateCommentRequest request)
    {
        var response = await _commentsApi.CreateAsync(request, boardId);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<CommentModel>(content));
    }

    public async Task<Result<object>> UpdateAsync(int boardId, int id, UpdateCommentRequest request)
    {
        var response = await _commentsApi.UpdateAsync(request, boardId, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> DeleteAsync(int boardId, int id)
    {
        var response = await _commentsApi.DeleteAsync(boardId, id);

        return response.HandleResult();
    }
}