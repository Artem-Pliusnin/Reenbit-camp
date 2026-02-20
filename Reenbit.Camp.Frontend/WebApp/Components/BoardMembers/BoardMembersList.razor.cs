using System.Text.Json;
using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.Enums;
using Domain.Models.BoardMembers;
using Domain.Requests.BoardMembers;
using Domain.Responses.BoardMembers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;

namespace WebApp.Components.BoardMembers;

public partial class BoardMembersList : ComponentBase, IDisposable
{
    [Parameter, EditorRequired]
    public int BoardId { get; set; }
    
    [Parameter, EditorRequired]
    public EventCallback UpdateBoardContent { get; set; }
    
    [CascadingParameter(Name="CurrentUser")]
    private BoardMemberModel CurrentUser { get; set; }
    
    [Inject] 
    public IBoardMembersService BoardMembersService { get; set; } = default!;
    
    [Inject] 
    public IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public NavigationManager NavigationManager { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    
    private List<IDisposable> Subscriptions = new();
    
    private bool IsLoading;
    
    private List<BoardMemberModel> Members = new();

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
            
        var result = await BoardMembersService.GetByBoardAsync(BoardId);
        
        if (result.IsSuccess)
        {
            Members = result.Value;
        }

        IsLoading = false;
        
        Subscriptions.Add(HomeHubConnection
            .On<BoardMemberDto>(SubscribeHomeHubConstants.AddMember, AddMember));
        
        Subscriptions.Add(HomeHubConnection
            .On<BoardMemberDto>(SubscribeHomeHubConstants.DeletedMember, OnDeletedMember));
        
        Subscriptions.Add(HomeHubConnection
            .On<UpdatedCardMemberRoleDto>(SubscribeHomeHubConstants.UpdateMemberRole, OnUpdateMemberRole));
    }
    
    private bool IsNameHighlighted(BoardMemberModel memeber) => 
        memeber.User.Subscription?.SubscriptionPlan.IsNameHighlighted ?? false;
    
    private IEnumerable<BoardRole> GetAvailableRoles(BoardMemberModel member)
    {
        if (!CanChangeRole(member))
        {
            return  Enum.GetValues<BoardRole>().ToList();
        }
        
        return Enum.GetValues<BoardRole>()
            .Where(r =>
                    (int)r >= (int)CurrentUser.Role &&
                    r != BoardRole.Owner
            ).OrderBy(r => r);
    }

    private bool CanChangeRole(BoardMemberModel member)
    {
        return  member.Role != BoardRole.Owner &&
                member.Id != CurrentUser.Id &&
                (int)CurrentUser.Role < (int)member.Role;
    }
    
    private bool CanRemove(BoardMemberModel member)
    {
        return member.Id != CurrentUser.Id &&
               (CurrentUser.Role == BoardRole.Owner || 
                CurrentUser.Role == BoardRole.Admin ) &&
               (int)CurrentUser.Role < (int)member.Role;
    }

    private void AddMember(BoardMemberDto member)
    {
        if (!Members.Any(m => m.Id == member.Id))
        {
            var memberModel = Mapper.Map<BoardMemberModel>(member);
 
            Members.Add(memberModel);
            StateHasChanged();
        }
    }

    private void OnDeletedMember(BoardMemberDto member)
    {
        if (CurrentUser.Id == member.Id)
        {
            NavigationManager.NavigateTo($"/");
            return;
        }
        
        Members.RemoveAll(m => m.Id == member.Id);
        
        StateHasChanged();
    }
    
    private void OnUpdateMemberRole(UpdatedCardMemberRoleDto dto)
    {
        var member = Members.Find(m => m.Id == dto.MemberId);

        if (member is not null)
        {
            member.Role = (BoardRole)dto.RoleId;
            if (CurrentUser.Id == dto.MemberId)
            {
                CurrentUser.Role = (BoardRole)dto.RoleId;
            }
            StateHasChanged();
        }
    }
    
    
    private async Task OnRoleChanged(BoardMemberModel member)
    {
        var result = await BoardMembersService.UpdateRoleAsync(
            member.Id,
            new UpdateBoardMemberRoleRequest(
                member.Id, 
                member.Role)
        );

        if (result.IsSuccess)
        {
            await HomeHubConnection
                .SendAsync(
                    SendHomeHubConstants.UpdateMemberRole, 
                    new UpdatedCardMemberRoleDto(member.Id, (int)member.Role), 
                    BoardId);
        }
        
        StateHasChanged();
    }

    private async Task RemoveMember(BoardMemberModel member)
    {
        var result = await BoardMembersService.DeleteAsync(member.Id);

        if (result.IsSuccess)
        {
            Members.Remove(member);
            
            StateHasChanged();
            await UpdateBoardContent.InvokeAsync();
            
            await HomeHubConnection
                .SendAsync(SendHomeHubConstants.DeleteMember, member, BoardId);
        }
    }

    private async Task LeaveBoard()
    {
        var result = await BoardMembersService.DeleteAsync(CurrentUser.Id);

        if (result.IsSuccess)
        {
            if (result.Value.NewOwner != null)
            {
                await HomeHubConnection
                    .SendAsync(
                        SendHomeHubConstants.UpdateMemberRole, 
                        new UpdatedCardMemberRoleDto(
                            result.Value.NewOwner.Id, 
                            (int)result.Value.NewOwner.Role), 
                        BoardId);
            }
            
            await HomeHubConnection
                .SendAsync(SendHomeHubConstants.DeleteMember, CurrentUser, BoardId);
            
            NavigationManager.NavigateTo($"/");
        }
    }
    
    public void Dispose()
    {
        foreach (var subscription in Subscriptions)
        {
            subscription.Dispose();
        }
    }
}