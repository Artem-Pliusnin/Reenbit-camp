namespace Application.Abstractions.Services;

public interface IChatService
{
    Task<bool> ImportAsync(
        Stream content,
        string documentId,
        string fileName);
    
    void StartChatSession(int userId);

    bool EndChatSession(int userId);

    Task<string> AskAsync(int userId, string question);
}