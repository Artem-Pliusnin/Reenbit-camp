using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.Models.BoardMembers;
using Domain.Models.Boards;
using Domain.Models.CardMembers;
using Domain.Models.Labels;
using Domain.Models.Users;
using Domain.Requests.CardMembers;
using Domain.Responses.BoardMembers;
using Domain.Responses.CardMembers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;

namespace WebApp.Components.CardMembers;

public partial class CardMembersSection : ComponentBase
{
    [CascadingParameter]
    public BoardInfoModel Board { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public int CardId { get; set; }
    
    [Parameter, EditorRequired] 
    public EventCallback<List<CardMemberModel>> OnUpdateMembers { get; set; }
    
    [Inject] 
    private IUsersService UsersService { get; set; } = default!;
    
    [Inject] 
    private ICardMembersService CardMembersService { get; set; } = default!;
    
    [Inject] 
    private IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    private HubConnection TaskHubConnection;
    
    private List<IDisposable> Subscriptions = new();
    
    private List<CardMemberModel> CardMembers { get; set; } = new();
    
    private List<UserModel> AvailableUsers { get; set; } = new();
    
    private TelerikPopover? PopoverRef { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
        TaskHubConnection = HubConnectionManager.Get(HubType.TaskHub);
        
        var cardMemberResult = await CardMembersService
            .GetByCardAsync(CardId);

        if (cardMemberResult.IsSuccess)
        {
            CardMembers = cardMemberResult.Value;
            await OnUpdateMembers.InvokeAsync(CardMembers);
        }
        
        var availableUsersResult = await UsersService
            .GetNotConnectedToCardAsync(CardId);

        if (availableUsersResult.IsSuccess)
        {
            AvailableUsers = availableUsersResult.Value;
        }
        
        Subscriptions.Add(HomeHubConnection
            .On<BoardMemberDto>(SubscribeHomeHubConstants.AddMember, AddNewMember));
        
        Subscriptions.Add(HomeHubConnection
            .On<BoardMemberDto>(SubscribeHomeHubConstants.DeletedMember, OnDeletedMember));
        
        Subscriptions.Add(TaskHubConnection
            .On<CardMemberDto>(SubscribeTaskHubConstants.AddCardMember, AddCardMember));

        Subscriptions.Add(TaskHubConnection
            .On<CardMemberDto>(SubscribeTaskHubConstants.DeleteCardMember, RemoveCardMember));
        
    }
    
    private void OpenMembersPopover()
    {
        PopoverRef?.Show();
    }
    
    private void CloseMembersPopover()
    {
        PopoverRef?.Hide();
    }

    private async Task RemoveMember(CardMemberModel member)
    {
        var result = await CardMembersService
            .DeleteAsync(member.Id);

        if (result.IsSuccess)
        {
            CardMembers.Remove(member);
            AvailableUsers.Add(member.User);
            await OnUpdateMembers.InvokeAsync(CardMembers);
            
            await TaskHubConnection
                .SendAsync(
                    SendTaskHubConstants.DeleteCardMember, 
                    member, 
                    CardId);
        }
        
        PopoverRef?.Refresh();
    }
    
    private async Task AddMember(UserModel user)
    {
        var result = await CardMembersService
            .CreateAsync(new CreateCardMemberRequest(CardId, user.Id));

        if (result.IsSuccess)
        {
            AvailableUsers.Remove(user);
            CardMembers.Add(result.Value);
            await OnUpdateMembers.InvokeAsync(CardMembers);
            
            await TaskHubConnection
                .SendAsync(
                    SendTaskHubConstants.AddCardMember, 
                    result.Value, 
                    CardId);
        }
        
        PopoverRef?.Refresh();
    }
    
    private void AddNewMember(BoardMemberDto member)
    {
        if (!AvailableUsers.Any(m => m.Id == member.User.Id))
        {
            var user = Mapper.Map<UserModel>(member.User);
 
            AvailableUsers.Add(user);
            PopoverRef?.Refresh();
        }
    }

    private void OnDeletedMember(BoardMemberDto member)
    {
        AvailableUsers.RemoveAll(m => m.Id == member.User.Id);
        CardMembers.RemoveAll(cm => cm.User.Id == member.User.Id);
        
        StateHasChanged();
        PopoverRef?.Refresh();
    }
    
    private void AddCardMember(CardMemberDto dto)
    {
        var cardMemberModel = Mapper.Map<CardMemberModel>(dto);
        
        AvailableUsers.RemoveAll(u => u.Id == cardMemberModel.User.Id);

        if (!CardMembers.Any(cl => cl.Id == dto.Id))
        {
            CardMembers.Add(cardMemberModel);
            StateHasChanged();
        }
        
        PopoverRef?.Refresh();
    }
    
    private void RemoveCardMember(CardMemberDto dto)
    {
        var cardMemberModel = Mapper.Map<CardMemberModel>(dto);
        
        CardMembers.RemoveAll(cm => cm.Id == dto.Id);
        
        if (!AvailableUsers.Any(u => u.Id == cardMemberModel.User.Id))
        {
            AvailableUsers.Add(cardMemberModel.User);
        }
        
        StateHasChanged();
        PopoverRef?.Refresh();
    }
}