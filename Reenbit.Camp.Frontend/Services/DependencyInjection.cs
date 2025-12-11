using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using WebApp.Authentication;
using Services.Abstractions.Services;
using Services.API;
using Services.APIServices;
using Services.Handlers;

namespace Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();

        services.AddScoped<IAuthService, AuthService>();
        
        services.AddScoped<TokenHandler>();

        var apiBaseUrl = configuration["ApiBaseUrl"] ?? 
                         throw new Exception("Api url not configured");;
        
        services.AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();

        return services;
    }
}