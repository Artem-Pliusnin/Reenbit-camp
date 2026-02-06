using Application.Abstractions.Services;
using Domain.Constants.ArchivationConstants;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Services;

public class ArchivationLogsService : IArchivationLogsService
{
    private readonly Container _container;

    public ArchivationLogsService(CosmosClient client)
    {
        var db = client.GetDatabase(ArchivationConstants.LogsDatabaseName);
        _container = db.GetContainer(ArchivationConstants.LogsContainerName);
    }
    
    public async Task SaveArchivationLogAsync(int boardId, ArchiveStatus status)
    {
        ArchivationLog log = new()
        {
            boardId = boardId,
            statusId = (int)status,
            statusName = status.ToString()
        };
        
        await _container.CreateItemAsync(
            log,
            new PartitionKey(log.boardId));
    }
}