using Microsoft.AspNetCore.Components;

namespace WebApp.Components.FAQChat;

public partial class ChatOpener : ComponentBase
{
    private bool IsOpen;

    void ToggleChat()
    {
        IsOpen = !IsOpen;
    }
}