using Domain.Models.CardMembers;
using Microsoft.AspNetCore.Components;

namespace WebApp.Components.CardMembers;

public partial class CardMemberRow : ComponentBase
{
    [Parameter, EditorRequired] 
    public CardMemberModel CardMember { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public EventCallback<CardMemberModel> OnDelete { get; set; }
    
    private void DeletMember()
    {
        OnDelete.InvokeAsync(CardMember);
    }
}