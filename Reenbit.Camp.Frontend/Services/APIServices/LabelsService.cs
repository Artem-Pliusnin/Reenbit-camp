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

    public async Task<Result<List<LabelModel>>> GetNotConnectedAsync(int cardId)
    {
        var response = await _labelsApi.GetNotConnectedAsync(cardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<LabelModel>>(content));
    }

    public async Task<Result<LabelModel>> CreateAsync(CreateLabelRequest request)
    {
        var response = await _labelsApi.CreateAsync(request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<LabelModel>(content));
    }

    public async Task<Result<object>> UpdateAsync(int id, UpdateLabelRequest request)
    {
        var response = await _labelsApi.UpdateAsync(request, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> DeleteAsync(int id)
    {
        var response = await _labelsApi.DeleteAsync(id);

        return response.HandleResult();
    }
}