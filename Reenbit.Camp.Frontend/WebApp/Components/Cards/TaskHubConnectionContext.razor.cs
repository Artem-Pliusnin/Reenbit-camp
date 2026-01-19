using Domain.Constants.HubConstants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Services.HubServices;

namespace WebApp.Components.Cards;

public partial class TaskHubConnectionContext : ComponentBase, IAsyncDisposable
{
    [Parameter, EditorRequired]
    public int CardId { get; set; }
    
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!; 
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection TaskHubConnection;
    
    protected override async Task OnInitializedAsync()
    {
        TaskHubConnection = HubConnectionManager.Get(HubType.TaskHub);
        
        await TaskHubConnection
            .SendAsync(SendTaskHubConstants.AddToTaskGroup, CardId);
    }

    public async ValueTask DisposeAsync()
    {
        if (TaskHubConnection is not null &&
            TaskHubConnection.State == HubConnectionState.Connected)
        {
            await TaskHubConnection
                .SendAsync(SendTaskHubConstants.DeleteFromTaskGroup, CardId);
        }
    }


}