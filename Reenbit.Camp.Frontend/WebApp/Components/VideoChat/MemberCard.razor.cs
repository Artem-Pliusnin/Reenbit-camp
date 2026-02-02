using Domain.Models.Users;
using Microsoft.AspNetCore.Components;

namespace WebApp.Components.VideoChat;

public partial class MemberCard : ComponentBase
{
    [Parameter, EditorRequired]
    public UserModel User { get; set; }
}