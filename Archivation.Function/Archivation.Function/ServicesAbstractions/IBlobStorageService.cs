using Archivation.Function.Models.DTos;

namespace Archivation.Function.ServicesAbstractions;

public interface IBlobStorageService
{
    Task UploadJsonAsync(string fileName, BoardDto board);
    
    Task<BoardDto?> DownloadJsonAsync(int boardId);
    
    Task DeleteBlobAsync(int boardId);
}