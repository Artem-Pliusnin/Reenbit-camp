using Domain.DTOs.Shared;

namespace Application.Abstractions.Services;

public interface IFileService
{
    Task<StoredFileDto> UploadAttachmentFileAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);
    
    Task DeleteAttachmentFileAsync(
        string fileName,
        CancellationToken cancellationToken = default);
}