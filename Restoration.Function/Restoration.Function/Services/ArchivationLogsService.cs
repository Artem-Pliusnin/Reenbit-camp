using Restoration.Function.Models.Enums;
using Restoration.Function.Models.Etities;
using Restoration.Function.ServicesAbstractions;
using Microsoft.Azure.Cosmos;

namespace Restoration.Function.Services;

public class ArchivationLogsService : IArchivationLogsService
{
    private readonly Container _container;

    public ArchivationLogsService(CosmosClient client)
    {
        var db = client.GetDatabase(RestorationConstants.LogsDatabaseName);
        _container = db.GetContainer(RestorationConstants.LogsContainerName);
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