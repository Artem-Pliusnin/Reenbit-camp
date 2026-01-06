using Domain.Models.Boards;
using Domain.Models.CardMembers;
using Domain.Models.Labels;
using Domain.Models.Users;
using Domain.Requests.CardMembers;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;
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

    private List<CardMemberModel> CardMembers { get; set; } = new();
    
    private List<UserModel> AvailableUsers { get; set; } = new();
    
    [Inject] 
    private IUsersService UsersService { get; set; } = default!;
    
    [Inject] 
    private ICardMembersService CardMembersService { get; set; } = default!;
    
    private TelerikPopover? PopoverRef { get; set; }
    
    
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
        }
        
        PopoverRef?.Refresh();
    }
    
    protected override async Task OnInitializedAsync()
    {
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
        
    }
}