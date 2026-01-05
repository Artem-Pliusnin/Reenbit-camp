using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration
            .GetConnectionString("PostgresConnectionString") 
            ?? throw new Exception("Connection string not found");;

        services.AddDbContext<TrelloAppDbContext>(options => 
            options.UseNpgsql(connectionString));

        services.AddScoped<TrelloAppDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IBoardMemberRepository, BoardMemberRepository>();
        services.AddScoped<IListRepository, ListRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<ILabelRepository, LabelRepository>();
        services.AddScoped<ICardLabelsRepository, CardLabelsRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}