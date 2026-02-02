using Domain.Models.CardAttachments;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using Services.Abstractions.Services;
using Services.API;
using Services.APIServices;
using Services.Authentication;
using Services.Handlers;
using Services.HubServices;

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
        services.AddScoped<IInvitationsService, InvitationsService>();
        services.AddScoped<IBoardMembersService, BoardMembersService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<ILabelsService, LabelsService>();
        services.AddScoped<ICardLabelsService, CardLabelsService>();
        services.AddScoped<ICardMembersService, CardMembersService>();
        services.AddScoped<ICommentsService, CommentsService>();
        services.AddScoped<ICardAttachmentService, CardAttachmentsService>();
        services.AddScoped<IFAQChatService, FAQChatService>();
        
        services.AddScoped<ITokenProvider, LocalStorageTokenProvider>();
        
        services.AddScoped<HubConnectionManager>();
        
        services.AddScoped<TokenHandler>();

        var apiBaseUrl = configuration["ApiUrls:ApiBaseUrl"] 
                         ?? throw new Exception("ApiBaseUrl not configured");

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
        
        services.AddRefitClient<IInvitationApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();

        services.AddRefitClient<IBoardMembersApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<IUsersApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<ILabelsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<ICardLabelsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<ICardMembersApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<ICommentsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<ICardAttachmentsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddRefitClient<IFAQChatApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<TokenHandler>();
        
        return services;
    }
}