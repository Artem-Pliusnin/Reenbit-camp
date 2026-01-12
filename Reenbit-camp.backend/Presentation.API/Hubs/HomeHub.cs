using System.Security.Claims;
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
        Console.WriteLine(listId);
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
    
    public static string GetBoardGroupName(int boardId)
    {
        return $"chat_{boardId}";
    }
}