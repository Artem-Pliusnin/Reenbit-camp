using Domain.DTOs.Shared;

namespace Application.Abstractions.Services;

public interface IFileService
{
    Task<StoredFileDto> UploadFileAsync(
        Stream content,
        string fileName,
        string contentType,
        string directoryName,
        CancellationToken cancellationToken = default);
    
    Task DeleteFileAsync(
        string fileName,
        string directoryName,
        CancellationToken cancellationToken = default);
}