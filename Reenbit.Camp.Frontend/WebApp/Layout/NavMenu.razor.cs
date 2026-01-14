using Microsoft.AspNetCore.Components;

namespace WebApp.Layout;

public partial class NavMenu
{
    [Parameter, EditorRequired] 
    public string Username { get; set; }

    private bool isOpen;

    private void ToggleMenu()
    {
        isOpen = !isOpen;
    }
}