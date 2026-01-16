using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Services.HubServices;

namespace WebApp.Components.Shared;

public partial class SocketHubContext : ComponentBase, IAsyncDisposable
{
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!; 
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    [Inject] 
    public NavigationManager NavigationManager { get; set; } = default!;

    private bool isConnected;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await HubConnectionManager.StartAsync(HubType.HomeHub);
            await HubConnectionManager.StartAsync(HubType.TaskHub);

            isConnected = true;
        }
        catch
        {
            NavigationManager.NavigateTo("/error", replace: true);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await HubConnectionManager.DisposeAsync(HubType.HomeHub);
    }
    
    
}