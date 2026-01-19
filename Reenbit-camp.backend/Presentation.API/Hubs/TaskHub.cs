using System.Text.Json;
using Domain.Constants.HubConstants;
using Domain.DTOs.CardAttachments;
using Domain.DTOs.CardLabels;
using Domain.DTOs.CardMembers;
using Domain.DTOs.Cards;
using Domain.DTOs.Comments;
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
    
    public async Task AddComment(CommentDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.AddComment, dto);
    }
    
    public async Task UpdateComment(CommentDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.UpdateComment, dto);
    }
    
    public async Task DeleteComment(int commentId, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.DeleteComment, commentId);
    }
    
    public async Task AddAttachment(CardAttachmentDto dto, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.AddAttachment, dto);
    }
    
    public async Task DeleteAttachment(int attachmentId, int cardId)
    {
        await Clients.OthersInGroup(GetTaskGroupName(cardId))
            .SendAsync(TaskHubConstants.DeleteAttachment, attachmentId);
    }
    public static string GetTaskGroupName(int taskId)
    {
        return $"task_{taskId}";
    }
}