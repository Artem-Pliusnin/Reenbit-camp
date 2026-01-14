using System.Text.Json;
using Domain.Constants.HubConstants;
using Domain.DTOs.CardLabels;
using Domain.DTOs.CardMembers;
using Domain.DTOs.Cards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentation.API.Hubs;

[Authorize]
public class TaskHub : Hub
{
    public async Task AddToTaskGroup(int taskId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetTaskGroupName(taskId));
    }
    
    public async Task DeleteFromTaskGroup(int taskId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetTaskGroupName(taskId));
    }
    
    public async Task UpdateCardTitle(UpdatedCardTitleDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.UpdateCardTitle, dto);
    }
    
    public async Task UpdateCardDescription(UpdatedCardDescriptionDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.UpdateCardDescription, dto);
    }
    
    public async Task UpdateCardDates(UpdatedCardDatesDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.UpdateCardDates, dto);
    }
    
    public async Task UpdateCardStatus(UpdatedCardStatusDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.UpdateCardStatus, dto);
    }
    
    public async Task AddCardLabel(CardLabelDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.AddCardLabel, dto);
    }
    
    public async Task DeleteCardLabel(CardLabelDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.DeleteCardLabel, dto);
    }
    
    public async Task AddCardMember(CardMemberDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.AddCardMember, dto);
    }
    
    public async Task DeleteCardMember(CardMemberDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.DeleteCardMember, dto);
    }
    
    public static string GetTaskGroupName(int taskId)
    {
        return $"task_{taskId}";
    }
}