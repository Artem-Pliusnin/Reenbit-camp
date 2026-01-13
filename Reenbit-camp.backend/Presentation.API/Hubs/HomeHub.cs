using System.Security.Claims;
using System.Text.Json;
using Domain.DTOs.Cards;
using Domain.DTOs.Labels;
using Domain.DTOs.Lists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentation.API.Hubs;

[Authorize]
public class HomeHub : Hub
{
    public async Task AddToBoardGroup(int boardId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetBoardGroupName(boardId));
    }
    
    public async Task DeleteFromBoardGroup(int boardId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetBoardGroupName(boardId));
    }
    
    public async Task AddNewLabel(LabelDto label, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("AddNewLabel", label);
    }
    
    public async Task DeleteLabel(int labelId, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("RemoveLabel", labelId);
    }
    
    public async Task UpdateListPosition(MoveListDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("MoveList", dto);
    }
    
    public async Task DeleteList(int listId, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("DeleteList", listId);
    }
    
    public async Task UpdateList(UpdateListDto list, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("UpdateList", list);
    }
    
    public async Task AddList(ListDto list, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("AddList", list);
    }
    
    public async Task CardsReordered(CardsReorderedDto dto)
    {
        await Clients
            .OthersInGroup(GetBoardGroupName(dto.BoardId))
            .SendAsync("CardsReordered", dto);
    }
    
    public async Task AddCard(CardDto card, int listId,  int boardId)
    {
        var dto = new CreatedCardDto()
        {
            ListId = listId,
            Card = card
        };
        
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("AddCard", dto);
    }
    
    public async Task DeleteCard(int cardId, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("RemoveCard", cardId);
    }
    
    public async Task UpdateCardTitle(UpdatedCardTitleDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("UpdateCardTitle", dto);
    }
    
    public async Task UpdateCardDates(UpdatedCardDatesDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("UpdateCardDates", dto);
    }
    
    public async Task UpdateCardStatus(UpdatedCardStatusDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("UpdateCardStatus", dto);
    }
    
    public async Task UpdateCardLabels(UpdatedCardLabelsDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("UpdateCardLabels", dto);
    }
    
    public async Task UpdateCardMembers(UpdatedCardMembersDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync("UpdateCardMembers", dto);
    }
    
    public static string GetBoardGroupName(int boardId)
    {
        return $"chat_{boardId}";
    }
}