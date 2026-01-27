using System.Text.Json;
using Application.Abstractions.Services;
using Domain.Constants.FIleConstants;
using Domain.DTOs.FAQChat;
using Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using MongoDB.Bson.IO;

namespace Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly Kernel _kernel;
    private readonly IKernelMemory _memory;
    private readonly IChatHistoryService _chatHistoryService;

    public ChatService(
        IOptions<AzureOpenAISettings> openAIOptions,
        IKernelMemory memory, 
        IChatHistoryService chatHistoryService)
    {
        _memory = memory;
        _chatHistoryService = chatHistoryService;
        
        var openAI = openAIOptions.Value;

        _kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: openAI.DeploymentChatName,
                endpoint: openAI.Endpoint,
                apiKey: openAI.ApiKey)
            .Build();
        
        var memoryPlugin = new MemoryPlugin(
            _memory,
            waitForIngestionToComplete: true);

        _kernel.ImportPluginFromObject(memoryPlugin, "memory");
    }

    public async Task<bool> ImportAsync(Stream content, string documentId, string fileName)
    {
        try
        {
            await _memory.ImportDocumentAsync(
                content: content, 
                documentId: documentId, 
                fileName: fileName,
                index: FAQConstants.FAQIndex);
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task StartChatSession(int userId)
    {
        await _chatHistoryService.ClearAsync(userId);
    }

    public async Task EndChatSession(int userId)
    {
        await _chatHistoryService.ClearAsync(userId);
    }

    public async Task<string> AskAsync(int userId ,string question)
    {
        OpenAIPromptExecutionSettings settings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        };
        
        var historyDtos = await _chatHistoryService.GetAsync(userId);

        var chatHistory = new ChatHistory();
        foreach (var m in historyDtos)
        {
            if (!string.IsNullOrWhiteSpace(m.Content))
            {
                chatHistory.AddMessage(
                    m.Role == "user" ? AuthorRole.User : AuthorRole.Assistant,
                    m.Content);
            }
        }

        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        
        var prompt = $@"
            Question to Kernel Memory: {question}

            Kernel Memory Answer: {{{{memory.ask question=""{question}"" index=""{FAQConstants.FAQIndex}""}}}}

            If the answer is empty say 'I don't know',otherwise reply with the answer.
            ";

        Console.WriteLine(JsonSerializer.Serialize(chatHistory));
        chatHistory.AddMessage(AuthorRole.User, prompt);
        
        var result = await chatCompletionService
            .GetChatMessageContentAsync(chatHistory, settings, _kernel);
        
        historyDtos.Add(new ChatMessageDto { Role = "user", Content = question });
    
        if (!string.IsNullOrWhiteSpace(result.Content))
        {
            historyDtos.Add(new ChatMessageDto { Role = "assistant", Content = result.Content });
        }

        await _chatHistoryService.SaveAsync(userId, historyDtos);

        return result.Content;
    }
}