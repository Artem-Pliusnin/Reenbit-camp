using System.Security.Claims;
using System.Text.Json;
using Domain.Constants.HubConstants;
using Domain.DTOs.BoardMembers;
using Domain.DTOs.Cards;
using Domain.DTOs.Invitations;
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
    
    public async Task AddInvitation(InvitationDto invitation, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.AddInvitation, invitation);
    }
    
    public async Task AddNewLabel(LabelDto label, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.AddNewLabel, label);
    }
    
    public async Task DeleteLabel(int labelId, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.RemoveLabel, labelId);
    }
    
    public async Task DeleteMember(BoardMemberDto member, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.DeletedMember, member);
    }
    
    public async Task UpdateMemberRole(UpdatedCardMemberRoleDto dto, int boardId)
    {
        Console.WriteLine(JsonSerializer.Serialize(dto));
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.UpdateMemberRole, dto);
    }
    
    public async Task UpdateListPosition(MoveListDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.MoveList, dto);
    }
    
    public async Task DeleteList(int listId, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.DeleteList, listId);
    }
    
    public async Task UpdateList(UpdateListDto list, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.UpdateList, list);
    }
    
    public async Task AddList(ListDto list, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.AddList, list);
    }
    
    public async Task CardsReordered(CardsReorderedDto dto)
    {
        await Clients
            .OthersInGroup(GetBoardGroupName(dto.BoardId))
            .SendAsync(HomeHubConstants.CardsReordered, dto);
    }
    
    public async Task AddCard(CardDto card, int listId,  int boardId)
    {
        var dto = new CreatedCardDto()
        {
            ListId = listId,
            Card = card
        };
        
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.AddCard, dto);
    }
    
    public async Task DeleteCard(int cardId, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.RemoveCard, cardId);
    }
    
    public async Task UpdateCardTitle(UpdatedCardTitleDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.UpdateCardTitle, dto);
    }
    
    public async Task UpdateCardDates(UpdatedCardDatesDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.UpdateCardDates, dto);
    }
    
    public async Task UpdateCardStatus(UpdatedCardStatusDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.UpdateCardStatus, dto);
    }
    
    public async Task UpdateCardLabels(UpdatedCardLabelsDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.UpdateCardLabels, dto);
    }
    
    public async Task UpdateCardMembers(UpdatedCardMembersDto dto, int boardId)
    {
        await Clients.OthersInGroup(GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.UpdateCardMembers, dto);
    }
    
    public static string GetBoardGroupName(int boardId)
    {
        return $"board_{boardId}";
    }
}