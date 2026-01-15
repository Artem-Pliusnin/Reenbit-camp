using Domain.Models.CardMembers;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace WebApp.Components.CardMembers;

public partial class CardMember : ComponentBase
{
    [Parameter, EditorRequired] 
    public CardMemberModel Member { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public bool CanDelete { get; set; }
    
    [Parameter, EditorRequired] 
    public EventCallback<CardMemberModel> OnDelete { get; set; }
    
    private TelerikPopover? PopoverRef { get; set; }
    
    private void OpenPopover()
    {
        PopoverRef?.Show();
    }
    
    private void ClosePopover()
    {
        PopoverRef?.Hide();
    }

    private void DeletMember()
    {
        OnDelete.InvokeAsync(Member);
    }
}