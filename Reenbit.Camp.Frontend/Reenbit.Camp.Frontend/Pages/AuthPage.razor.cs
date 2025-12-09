using Microsoft.AspNetCore.Components;

namespace Reenbit.Camp.Frontend.Pages;

public partial class AuthPage : ComponentBase
{
    private bool isAuthorizing = true;

    private void OnLoginChange(bool value) => isAuthorizing = value;
}