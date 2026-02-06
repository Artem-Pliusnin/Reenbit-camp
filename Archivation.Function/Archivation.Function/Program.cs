using Archivation.Function.Data;
using Archivation.Function.Data.Repositories;
using Archivation.Function.Services;
using Archivation.Function.Services.Mapping;
using Archivation.Function.ServicesAbstractions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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