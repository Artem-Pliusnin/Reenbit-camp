using Application.Abstractions.Services;
using Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.AddScoped<ITokenProvider, TokenProvider>();
        
        return services;
    }
}