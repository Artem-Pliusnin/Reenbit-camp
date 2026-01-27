using Microsoft.AspNetCore.SignalR;

namespace Presentation.API.Hubs;

public class VideoChatHub : Hub
{
    public async Task JoinBoard(int boardId)
    {
        var roomId = GetRoomGroupName(boardId);
        var userId = Context.UserIdentifier!;

        await Groups.AddToGroupAsync(Context.ConnectionId, GetRoomGroupName(boardId));
        await Clients.OthersInGroup(roomId).SendAsync("UserJoined", userId);
    }

    public async Task LeaveBoard(int boardId)
    {
        var roomId = GetRoomGroupName(boardId);
        var userId = Context.UserIdentifier!;

        await Clients.OthersInGroup(roomId)
            .SendAsync("UserLeft", userId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task SendOffer(string targetUserId, string offer)
    {
        await Clients.User(targetUserId)
            .SendAsync("ReceiveOffer", Context.UserIdentifier, offer);
    }

    public async Task SendAnswer(string targetUserId, string answer)
    {
        await Clients.User(targetUserId)
            .SendAsync("ReceiveAnswer", Context.UserIdentifier, answer);
    }

    public async Task SendIce(string targetUserId, string candidate)
    {
        await Clients.User(targetUserId)
            .SendAsync("ReceiveIce", Context.UserIdentifier, candidate);
    }

    public static string GetRoomGroupName(int boardId)
    {
        return $"room_{boardId}";
    }
}