using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restoration.Function.Data;
using Restoration.Function.Data.Repositories;
using Restoration.Function.Services;
using Restoration.Function.Services.Mapping;
using Restoration.Function.ServicesAbstractions;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

/*builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();*/

string connectionString = builder.Configuration["PostgresConnectionString"];

string storageConnectionString = builder.Configuration["AzureStorageConnectionString"];
        
builder.Services.AddAzureClients(azureBuilder =>
{
    azureBuilder.AddBlobServiceClient(storageConnectionString);
});

builder.Services.AddDbContext<BoardsDbContext>(options => 
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IBoardArchiveRepository, BoardArchiveRepository>();

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

builder.Services.AddAutoMapper(typeof(ArchivationProfile));

builder.Services.AddSingleton(s =>
{
    var client = new CosmosClient(
        builder.Configuration["CosmosConnectionString"]);

    return client;
});
        
builder.Services.AddScoped<IArchivationLogsService, ArchivationLogsService>();

builder.Build().Run();