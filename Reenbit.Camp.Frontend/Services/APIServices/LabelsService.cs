using AutoMapper;
using Domain.Models.Invitations;
using Domain.Models.Labels;
using Domain.Requests.Labels;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class LabelsService : ILabelsService
{
    private readonly ILabelsApi _labelsApi;
    private readonly IMapper _mapper;

    public LabelsService(ILabelsApi labelsApi, IMapper mapper)
    {
        _labelsApi = labelsApi;
        _mapper = mapper; 
    }
    
    public async Task<Result<List<LabelModel>>> GetByBoardAsync(int boardId)
    {
        var response = await _labelsApi.GetByBoardAsync(boardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<LabelModel>>(content));
    }

    public async Task<Result<List<LabelModel>>> GetNotConnectedAsync(int boardId, int cardId)
    {
        var response = await _labelsApi.GetNotConnectedAsync(boardId, cardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<LabelModel>>(content));
    }

    public async Task<Result<LabelModel>> CreateAsync(int boardId, CreateLabelRequest request)
    {
        var response = await _labelsApi.CreateAsync(request, boardId);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<LabelModel>(content));
    }

    public async Task<Result<object>> UpdateAsync(int boardId, int id, UpdateLabelRequest request)
    {
        var response = await _labelsApi.UpdateAsync(request, boardId, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> DeleteAsync(int boardId, int id)
    {
        var response = await _labelsApi.DeleteAsync(boardId, id);

        return response.HandleResult();
    }
}