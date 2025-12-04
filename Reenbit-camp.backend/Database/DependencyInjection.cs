using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration
            .GetConnectionString("PostgresConnectionString");

        services.AddSingleton(new DatabaseInitializer(connectionString));
        
        return services;
    }
}