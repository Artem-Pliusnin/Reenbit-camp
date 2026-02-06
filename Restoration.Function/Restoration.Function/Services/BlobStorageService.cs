using System.Text;
using System.Text.Json;
using Restoration.Function.Models.DTos;
using Restoration.Function.ServicesAbstractions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Restoration.Function;
using ArchiveStatus = Restoration.Function.Models.Enums.ArchiveStatus;

namespace Restoration.Function.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IArchivationLogsService _archivationLogsService;

    public BlobStorageService(BlobServiceClient blobServiceClient, 
        IArchivationLogsService archivationLogsService)
    {
        _blobServiceClient = blobServiceClient;
        _archivationLogsService = archivationLogsService;
    }
    
    public async Task<BoardDto?> DownloadJsonAsync(int boardId)
    {
        try
        {
            var container = _blobServiceClient
                .GetBlobContainerClient(RestorationConstants.BlobStorageContainerName);
            
            var blobClient = container.GetBlobClient(boardId.ToString());

            if (!await blobClient.ExistsAsync())
            {
                await _archivationLogsService
                    .SaveArchivationLogAsync(boardId, ArchiveStatus.FailedToGetFromBlobStorage);
                return null;
            }
            
            var stream = await blobClient.OpenReadAsync();
            
            var board = JsonSerializer.Deserialize<BoardDto>(stream);
            
            return board;
        }
        catch
        {
            await _archivationLogsService
                .SaveArchivationLogAsync(boardId, ArchiveStatus.FailedToGetFromBlobStorage);
            throw;
        }
    }

    public async Task DeleteBlobAsync(int boardId)
    {
        try
        {
            var container = _blobServiceClient
                .GetBlobContainerClient(RestorationConstants.BlobStorageContainerName);
            
            var blobClient = container.GetBlobClient(boardId.ToString());

            await blobClient.DeleteIfExistsAsync();
            
            await _archivationLogsService
                .SaveArchivationLogAsync(boardId, ArchiveStatus.DeletedFromBlobStorage);
        }
        catch
        {
            await _archivationLogsService
                .SaveArchivationLogAsync(boardId, ArchiveStatus.FailedToDeleteFromBlobStorage);
            
            throw;
        }
    }
}