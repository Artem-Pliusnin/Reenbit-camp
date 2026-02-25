using AutoMapper;
using Domain.Models.CardAttachments;
using Domain.Shared;
using Refit;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class CardAttachmentsService : ICardAttachmentService
{
    private readonly ICardAttachmentsApi _cardAttachmentsApi;
    private readonly IMapper _mapper;

    public CardAttachmentsService(ICardAttachmentsApi cardAttachmentsApi, IMapper mapper)
    {
        _cardAttachmentsApi = cardAttachmentsApi;
        _mapper = mapper; 
    }
    
    public async Task<Result<List<CardAttachmentModel>>> GetByCardAsync(int boardId, int cardId)
    {
        var response = await _cardAttachmentsApi.GetByCardAsync(boardId, cardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<CardAttachmentModel>>(content));
    }

    public async Task<Result<CardAttachmentModel>> CreateAsync(int boardId, int cardId, StreamPart file)
    {
        var response = await _cardAttachmentsApi.CreateAsync(boardId, cardId, file);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<CardAttachmentModel>(content));
    }

    public async Task<Result<object>> DeleteAsync(int boardId, int id)
    {
        var response = await _cardAttachmentsApi.DeleteAsync(boardId, id);

        return response.HandleResult();
    }
}