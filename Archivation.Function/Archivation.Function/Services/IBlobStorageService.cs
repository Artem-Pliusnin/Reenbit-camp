using Archivation.Function.Models.DTos;

namespace Archivation.Function.Services;

public interface IBlobStorageService
{
    Task<string> UploadJsonAsync(string fileName, BoardDto board);
    Task<bool> DeleteBlobAsync(string blobName);
    
    
}