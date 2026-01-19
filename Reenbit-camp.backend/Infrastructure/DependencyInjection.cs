using Application.Abstractions.Services;
using Infrastructure.Authentication;
using Infrastructure.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
        
        return services;
    }
}