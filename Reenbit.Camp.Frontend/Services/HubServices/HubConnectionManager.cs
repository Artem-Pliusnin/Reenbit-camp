using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Services.Abstractions.Services;

namespace Services.HubServices;

public class HubConnectionManager
{
    private readonly ITokenProvider _tokenProvider;
    private readonly string? _hubBaseUrl;    

    private readonly Dictionary<HubType, HubConnection> _connections = new();

    public HubConnectionManager(
        ITokenProvider tokenProvider,
        IConfiguration configuration)
    {
        _tokenProvider = tokenProvider;
        _hubBaseUrl = configuration["ApiUrls:HubBaseUrl"];
    }

    public HubConnection Get(HubType type)
    {
        if (_connections.TryGetValue(type, out var hub))
        {
            return hub;
        }

        var hubUrl = type switch
        {
            HubType.HomeHub => $"{_hubBaseUrl}/home",
            HubType.TaskHub => $"{_hubBaseUrl}/task",
            HubType.VideoChatHub => $"{_hubBaseUrl}/video-chat"
        };

        hub = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider =
                    () => _tokenProvider.GetAccessTokenAsync();
            })
            .Build();
        
        _connections[type] = hub;
        return hub;
    }

    public async Task StartAsync(HubType type)
    {
        var hub = Get(type);
        
        if (hub.State == HubConnectionState.Disconnected)
        {
            await hub.StartAsync();
        }
    }
    
    public async Task DisposeAsync(HubType type)
    {
        if (_connections.TryGetValue(type, out var hub))
        {
            if (hub.State != HubConnectionState.Disconnected)
            {
                await hub.StopAsync();
            }

            await hub.DisposeAsync();
            _connections.Remove(type);
        }
    }
    
}