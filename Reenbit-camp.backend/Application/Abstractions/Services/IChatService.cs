namespace Application.Abstractions.Services;

public interface IChatService
{
    Task<bool> ImportAsync(
        Stream content,
        string documentId,
        string fileName);
    
    Task StartChatSession(int userId);

    Task EndChatSession(int userId);

    Task<string> AskAsync(int userId, string question);
}