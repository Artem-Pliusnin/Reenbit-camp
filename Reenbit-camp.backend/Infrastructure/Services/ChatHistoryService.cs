using System.Text.Json;
using Application.Abstractions.Services;
using Domain.DTOs.FAQChat;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class ChatHistoryService : IChatHistoryService
{
    private readonly IDatabase _db;

    public ChatHistoryService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<List<ChatMessageDto>> GetAsync(int userId)
    {
        var value = await _db.StringGetAsync(GetKey(userId));
        if (value.HasValue)
        {
            return JsonSerializer.Deserialize<List<ChatMessageDto>>(value);
        }
        
        return new List<ChatMessageDto>();
    }

    public async Task SaveAsync(int userId, List<ChatMessageDto> messages)
    {
        var data = JsonSerializer.Serialize(messages);
        await _db.StringSetAsync(
            GetKey(userId),
            data);
    }

    public async Task ClearAsync(int userId)
    {
        await _db.KeyDeleteAsync(GetKey(userId));
    }

    private static string GetKey(int userId)
    {
        return $"chat:history:{userId}";
    }
}