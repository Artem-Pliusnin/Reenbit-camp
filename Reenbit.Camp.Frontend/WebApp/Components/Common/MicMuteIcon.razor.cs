using Microsoft.AspNetCore.Components;

namespace WebApp.Components.Common;

public partial class MicMuteIcon : ComponentBase
{
    [Parameter]
    public string Class { get; set; } = string.Empty;
}