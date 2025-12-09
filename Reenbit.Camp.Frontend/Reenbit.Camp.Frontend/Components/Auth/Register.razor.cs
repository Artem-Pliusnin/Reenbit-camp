using Microsoft.AspNetCore.Components;
using Reenbit.Camp.Frontend.Models.Auth;

namespace Reenbit.Camp.Frontend.Components.Auth;

public partial class Register : ComponentBase
{
    private RegisterModel model = new();
    
    private async Task OnRegisterAsync()
    {
        Console.WriteLine("User registered:");
    }
}