using Domain.Enums;
using Domain.Models.BoardMembers;
using Domain.Requests.BoardMembers;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Components.BoardMembers;

public partial class BoardMembersList : ComponentBase
{
    [Parameter, EditorRequired]
    public int BoardId { get; set; }
    
    [CascadingParameter(Name="CurrentUser")]
    private BoardMemberModel CurrentUser { get; set; }
    
    [Inject] 
    public IBoardMembersService BoardMembersService { get; set; } = default!;
    
    private bool IsLoading;
    
    private List<BoardMemberModel> Members = new();

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        var result = await BoardMembersService.GetByBoardAsync(BoardId);
        
        if (result.IsSuccess)
        {
            Members = result.Value;
        }

        IsLoading = false;
    }
    
    private IEnumerable<BoardRole> GetAvailableRoles(BoardMemberModel member)
    {
        if (member.Role == BoardRole.Owner)
        {
            return new List<BoardRole>(){ BoardRole.Owner };
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
    
    private async Task OnRoleChanged(BoardMemberModel member)
    {
            await BoardMembersService.UpdateRoleAsync(
            member.Id,
            new UpdateBoardMemberRoleRequest(
                member.Id, 
                member.Role)
        );
        
        StateHasChanged();
    }
}