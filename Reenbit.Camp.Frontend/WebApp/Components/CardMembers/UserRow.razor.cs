using Domain.Models.Users;
using Microsoft.AspNetCore.Components;

namespace WebApp.Components.CardMembers;

public partial class UserRow : ComponentBase
{
    [Parameter, EditorRequired] 
    public UserModel User { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public EventCallback<UserModel> OnAdd { get; set; }
    
    private void AddMember()
    {
        OnAdd.InvokeAsync(User);
    }
}