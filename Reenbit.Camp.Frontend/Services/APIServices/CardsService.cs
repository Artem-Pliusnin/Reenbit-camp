using AutoMapper;
using Domain.Models.Cards;
using Domain.Requests.Cards;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class CardsService : ICardsService
{
    private readonly ICardsApi _cardsApi;
    private readonly IMapper _mapper;

    public CardsService(ICardsApi cardsApi, IMapper mapper)
    {
        _cardsApi = cardsApi;
        _mapper = mapper;
    }

    
    public async Task<Result<CardModel>> CreateAsync(CreateCardRequest request)
    {
        var response = await _cardsApi.CreateAsync(request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<CardModel>(content));
    }

    public async Task<Result<List<CardModel>>> GetByListAsync(int listId)
    {
        var response = await _cardsApi.GetByListAsync(listId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<CardModel>>(content));
    }

    public async Task<Result<CardInfoModel>> GetInfoAsync(int id)
    {
        var response = await _cardsApi.GetInfoAsync(id);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<CardInfoModel>(content));
    }

    public async Task<Result<object>> UpdateAsync(int id, UpdateCardRequest request)
    {
        var response = await _cardsApi.UpdateAsync(request, id);

        return response.HandleResult();
    }
    
    public async Task<Result<object>> UpdateStatusAsync(int id, UpdateCardStatusRequest request)
    {
        var response = await _cardsApi.UpdateStatusAsync(request, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> UpdatePositionAsync(int id, UpdateCardPositionRequest request)
    {
        var response = await _cardsApi.UpdatePositionAsync(request, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> UpdateDeadlineAsync(int id, UpdateCardDeadlineRequest request)
    {
        var response = await _cardsApi.UpdateDeadlineAsync(request, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> DeleteAsync(int id)
    {
        var response = await _cardsApi.DeleteAsync(id);

        return response.HandleResult();
    }
}