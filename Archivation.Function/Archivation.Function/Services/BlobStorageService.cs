using System.Text;
using System.Text.Json;
using Archivation.Function.Models.DTos;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Archivation.Function.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadJsonAsync(string fileName,  BoardDto board)
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

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<bool> DeleteBlobAsync(string blobName)
    {
        throw new NotImplementedException();
    }
}