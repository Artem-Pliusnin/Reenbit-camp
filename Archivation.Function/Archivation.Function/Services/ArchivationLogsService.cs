using Archivation.Function;
using Archivation.Function.Models.Enums;
using Archivation.Function.Models.Etities;
using Archivation.Function.ServicesAbstractions;
using Microsoft.Azure.Cosmos;

namespace Archivation.Function.Services;

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