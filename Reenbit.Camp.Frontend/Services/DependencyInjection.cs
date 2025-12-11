using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Authentication;
using Services.Abstractions.Services;
using Services.APIServices;
using Services.Handlers;

namespace Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
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

        services.AddHttpClient<ApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7011/api/");
        }).AddHttpMessageHandler<TokenHandler>();

        services.AddHttpClient("RefreshClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7011/api/");
        });

        return services;
    }
}