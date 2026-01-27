using Domain.DTOs.FAQChat;

namespace Application.Abstractions.Services;

public interface IChatHistoryService
{
    Task<List<ChatMessageDto>> GetAsync(int userId);
    
    Task SaveAsync(int userId, List<ChatMessageDto> messages);
    
    Task ClearAsync(int userId);
}