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

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            var newCard = _mapper.Map<CardModel>(response.Content);
            return newCard;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<CardModel>(error);
    }

    public async Task<Result<List<CardModel>>> GetByListAsync(int listId)
    {
        var response = await _cardsApi.GetByListAsync(listId);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            var cards = _mapper.Map<List<CardModel>>(response.Content);
            return cards;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<List<CardModel>>(error);
    }

    public async Task<Result<object>> UpdateAsync(int id, UpdateCardRequest request)
    {
        var response = await _cardsApi.UpdateAsync(request, id);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<object>(error);
    }

    public async Task<Result<object>> UpdatePositionAsync(int id, UpdateCardPositionRequest request)
    {
        var response = await _cardsApi.UpdatePositionAsync(request, id);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<object>(error);
    }

    public async Task<Result<object>> UpdateDeadlineAsync(int id, UpdateCardDeadlineRequest request)
    {
        var response = await _cardsApi.UpdateDeadlineAsync(request, id);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<object>(error);
    }

    public async Task<Result<object>> DeleteAsync(int id)
    {
        var response = await _cardsApi.DeleteAsync(id);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<object>(error);
    }
}