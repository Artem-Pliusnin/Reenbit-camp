using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;

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

        services.AddDbContext<TrelloAppDbContext>(
            options => options
                .UseNpgsql(connectionString));

        services.AddScoped<TrelloAppDbContext>();
        
        return services;
    }
}