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

    
    public async Task<Result<CardModel>> CreateAsync(int boardId, CreateCardRequest request)
    {
        var response = await _cardsApi.CreateAsync(boardId, request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<CardModel>(content));
    }

    public async Task<Result<List<CardModel>>> GetByListAsync(int boardId, int listId)
    {
        var response = await _cardsApi.GetByListAsync(boardId, listId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<CardModel>>(content));
    }

    public async Task<Result<CardInfoModel>> GetInfoAsync(int boardId, int id)
    {
        var response = await _cardsApi.GetInfoAsync(boardId, id);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<CardInfoModel>(content));
    }

    public async Task<Result<object>> UpdateAsync(int boardId, int id, UpdateCardRequest request)
    {
        var response = await _cardsApi.UpdateAsync(request, boardId, id);

        return response.HandleResult();
    }
    
    public async Task<Result<object>> UpdateStatusAsync(int boardId, int id, UpdateCardStatusRequest request)
    {
        var response = await _cardsApi.UpdateStatusAsync(request, boardId, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> UpdatePositionAsync(int boardId, int id, UpdateCardPositionRequest request)
    {
        var response = await _cardsApi.UpdatePositionAsync(request,boardId, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> UpdateDeadlineAsync(int boardId, int id, UpdateCardDeadlineRequest request)
    {
        var response = await _cardsApi.UpdateDeadlineAsync(request, boardId, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> DeleteAsync(int boardId, int id)
    {
        var response = await _cardsApi.DeleteAsync(boardId, id);

        return response.HandleResult();
    }
}