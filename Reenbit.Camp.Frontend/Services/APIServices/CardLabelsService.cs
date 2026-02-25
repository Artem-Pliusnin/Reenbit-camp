using AutoMapper;
using Domain.Models.Labels;
using Domain.Requests.Labels;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class CardLabelsService : ICardLabelsService
{
    private readonly ICardLabelsApi _cardLabelsApi;
    private readonly IMapper _mapper;

    public CardLabelsService(ICardLabelsApi cardLabelsApi, IMapper mapper)
    {
        _cardLabelsApi = cardLabelsApi;
        _mapper = mapper; 
    }
    
    public async Task<Result<List<CardLabelModel>>> GetByCardAsync(int boardId, int cardId)
    {
        var response = await _cardLabelsApi.GetByCardAsync(boardId, cardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<CardLabelModel>>(content));
    }

    public async Task<Result<CardLabelModel>> CreateAsync(int boardId, CreateCardLabelRequest request)
    {
        var response = await _cardLabelsApi.CreateAsync(boardId, request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<CardLabelModel>(content));
    }

    public async Task<Result<object>> DeleteAsync(int boardId, int id)
    {
        var response = await _cardLabelsApi.DeleteAsync(boardId, id);

        return response.HandleResult();
    }
}
