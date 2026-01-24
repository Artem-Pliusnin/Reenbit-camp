using Application.Abstractions.Services;
using Infrastructure.Authentication;
using Infrastructure.Configuration;
using Infrastructure.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var storageConnectionString = configuration["AzureStorage:ConnectionString"];
        
        services.AddAzureClients(azureBuilder =>
        {
            azureBuilder.AddBlobServiceClient(storageConnectionString);
        });
        
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.AddScoped<ITokenProvider, TokenProvider>();
        
        services.AddScoped<IFileService, AzureFileService>();
        services.AddScoped<IImageService, ImageService>();
        
        services.Configure<AzureOpenAISettings>(
            configuration.GetSection("AzureOpenAI"));
        
        services.Configure<AzureSearchSettings>(
            configuration.GetSection("AzureSearch"));
        
        services.AddSingleton<IKernelMemory>(sp =>
        {
            var openAISettings = sp
                .GetRequiredService<IOptions<AzureOpenAISettings>>()
                .Value;
            
            var searchSettings = sp
                .GetRequiredService<IOptions<AzureSearchSettings>>()
                .Value;
            
            var embeddingConfig = new AzureOpenAIConfig
            {
                APIKey = openAISettings.ApiKey,
                Deployment = openAISettings.DeploymentEmbeddingName,
                Endpoint = openAISettings.Endpoint,
                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey
            };

            var chatConfig = new AzureOpenAIConfig
            {
                APIKey = openAISettings.ApiKey,
                Deployment = openAISettings.DeploymentChatName,
                Endpoint = openAISettings.Endpoint,
                APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey
            };

            return new KernelMemoryBuilder()
                .WithAzureOpenAITextGeneration(chatConfig)
                .WithAzureOpenAITextEmbeddingGeneration(embeddingConfig)
                .WithAzureAISearchMemoryDb(searchSettings.Endpoint, searchSettings.ApiKey)
                .Build<MemoryServerless>();
        });
        
        services.AddScoped<IChatService, ChatService>();
        
        return services;
    }
}