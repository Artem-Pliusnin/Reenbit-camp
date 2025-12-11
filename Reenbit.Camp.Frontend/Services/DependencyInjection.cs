using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Authentication;
using Services.Abstractions.Services;
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
        services.AddScoped<IRefreshService, RefreshService>(sp =>
        {
            var client = sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("RefreshClient");
            return new RefreshService(client);
        });
        
        services.AddScoped<TokenHandler>();

        var apiBaseUrl = configuration["ApiBaseUrl"] ?? 
                         throw new Exception("Api url not configured");;
        
        services.AddHttpClient<ApiClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        }).AddHttpMessageHandler<TokenHandler>();

        services.AddHttpClient("RefreshClient", client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        });

        return services;
    }
}