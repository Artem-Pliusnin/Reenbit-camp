using Microsoft.AspNetCore.Components;
using Reenbit.Camp.Frontend.Models.Auth;

namespace Reenbit.Camp.Frontend.Components.Auth;

public partial class LogIn : ComponentBase
{
    private Login model = new();
    
    private Task HandleLogin()
    {
        Console.WriteLine("Login clicked!");
        return Task.CompletedTask;
    }
}