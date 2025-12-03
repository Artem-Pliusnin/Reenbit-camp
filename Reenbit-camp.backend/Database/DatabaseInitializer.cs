using System.Reflection;
using DbUp;

namespace Database;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        EnsureDatabase.For.PostgresqlDatabase(_connectionString);
        
        var upgrader = DeployChanges.To
            .PostgresqlDatabase(_connectionString)
            .WithScriptsEmbeddedInAssembly(AssemblyReference.Assembly)
            .LogToConsole()
            .Build();

        if (upgrader.IsUpgradeRequired())
        {
            var result = upgrader.PerformUpgrade();
        }
    }
}