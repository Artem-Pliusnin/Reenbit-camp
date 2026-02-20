using Application.Abstractions.Services;
using Azure.Messaging.ServiceBus;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.Configuration;
using Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using StackExchange.Redis;

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
        
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisEndpoint = configuration["Redis:Endpoint"];
            var redisPassword = configuration["Redis:Password"];
            var redisPort = configuration["Redis:Port"] ?? "6380";
            
            var options = new ConfigurationOptions
            {
                EndPoints = { $"{redisEndpoint}:{redisPort}" },
                Password = redisPassword,
                Ssl = true,
                AbortOnConnectFail = false,
                ConnectRetry = 5,
                ConnectTimeout = 15000,
                SyncTimeout = 15000,
                AsyncTimeout = 15000,
                KeepAlive = 60
            };

            return ConnectionMultiplexer.Connect(options);
        });
        
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IChatHistoryService, ChatHistoryService>();
        services.AddTransient<ArchivePendingBoardsJob>();
        
        string connectionString = configuration
                                      .GetConnectionString("PostgresConnectionString") 
                                  ?? throw new Exception("Connection string not found");
        
        services.AddHangfire(config => 
            config.UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options => 
                options.UseNpgsqlConnection(connectionString))
        );
        services.AddHangfireServer();

        services.AddSingleton<ServiceBusClient>(options =>
        {
            var connectionString = configuration["AzureServiceBus:ConnectionString"];
            return new ServiceBusClient(connectionString);
        });
        
        services.AddScoped<IMessageQueueService, AzureServiceBusService>();
        
        services.AddSingleton(s =>
        {
            var client = new CosmosClient(
                configuration["Cosmos:ConnectionString"]);

            return client;
        });
        
        services.AddScoped<IArchivationLogsService, ArchivationLogsService>();
        
        services.Configure<StripeSettings>(
            configuration.GetSection("Stripe"));

        services.AddScoped<IStripeService, StripeService>();
        
        return services;
    }
}