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
        services.AddAutoMapper(cfg => { }
            , typeof(DependencyInjection).Assembly);
        
        services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBoardsService, BoardsService>();
        services.AddScoped<IListsService, ListsService>();
        services.AddScoped<ICardsService, CardsService>();
        
        services.AddScoped<TokenHandler>();

        var apiBaseUrl = configuration["ApiBaseUrl"] ?? 
                         throw new Exception("Api url not configured");

        services.AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));
        
        services.AddRefitClient<IBoardsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<IListsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<ICardsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();

        return services;
    }
}