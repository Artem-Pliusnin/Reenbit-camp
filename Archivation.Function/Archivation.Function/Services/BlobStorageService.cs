using System.Text;
using System.Text.Json;
using Archivation.Function.Models.DTos;
using Archivation.Function.ServicesAbstractions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ArchiveStatus = Archivation.Function.Models.Enums.ArchiveStatus;

namespace Archivation.Function.Services;

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

    public async Task UploadJsonAsync(string fileName,  BoardDto board)
    {
        try
        {
            var container = _blobServiceClient
                .GetBlobContainerClient(ArchivationConstants.BlobStorageContainerName);
            
            await container.CreateIfNotExistsAsync();
            
            var blobClient = container.GetBlobClient(fileName);

            var json = JsonSerializer.Serialize(board, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var bytes = Encoding.UTF8.GetBytes(json);
            using var stream = new MemoryStream(bytes);

            await blobClient.UploadAsync(stream, new BlobHttpHeaders
            {
                ContentType = "application/json"
            });
            
            await _archivationLogsService
                .SaveArchivationLogAsync(board.Id, ArchiveStatus.SavedToBlobStorage);
        }
        catch(Exception ex)
        {
            await _archivationLogsService
                .SaveArchivationLogAsync(board.Id, ArchiveStatus.FailedToSaveToBlobStorage);
            throw;
        }
    }
    
    public async Task<BoardDto?> DownloadJsonAsync(int boardId)
    {
        try
        {
            var container = _blobServiceClient
                .GetBlobContainerClient(ArchivationConstants.BlobStorageContainerName);
            
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
                .GetBlobContainerClient(ArchivationConstants.BlobStorageContainerName);
            
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