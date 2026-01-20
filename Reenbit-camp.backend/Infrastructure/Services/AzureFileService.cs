using Application.Abstractions.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Domain.Constants.FIleConstants;
using Domain.DTOs.Shared;
using Domain.Exceptions;

namespace Infrastructure.Services;

public class AzureFileService : IFileService
{
    private readonly BlobServiceClient _blobServiceClient;
    
    public AzureFileService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<StoredFileDto> UploadFileAsync(
        Stream content, 
        string fileName, 
        string contentType,
        string directoryName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var container = _blobServiceClient
                .GetBlobContainerClient(directoryName);

            await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var uniqueFileName = $"{Guid.NewGuid()}-{fileName}";

            var blob = container.GetBlobClient(uniqueFileName);

            await blob.UploadAsync(
                content,
                new BlobHttpHeaders { ContentType = contentType },
                cancellationToken: cancellationToken);
            
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = container.Name,
                BlobName = uniqueFileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddYears(100)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasUri = blob.GenerateSasUri(sasBuilder);

            return new StoredFileDto
            {
                FileName = uniqueFileName,
                FileUrl = sasUri.ToString()
            };
        }
        catch (Exception ex)
        {
            throw new FileStorageException("Error occured while saving file.", ex);
        }
    }
    
    public async Task DeleteFileAsync(
        string fileName,
        string directoryName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var container = _blobServiceClient
                .GetBlobContainerClient(directoryName);

            var blob = container.GetBlobClient(fileName);

            await blob.DeleteIfExistsAsync(
                DeleteSnapshotsOption.IncludeSnapshots,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            throw new FileStorageException(
                "Error occured while deleting file.", ex);
        }
    }
    
}