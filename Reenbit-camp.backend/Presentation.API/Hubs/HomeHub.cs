using System.Security.Claims;
using Domain.DTOs.Labels;
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
    
    public static string GetBoardGroupName(int boardId)
    {
        return $"chat_{boardId}";
    }
}