using Restoration.Function.Models.DTos;

namespace Restoration.Function.ServicesAbstractions;

public interface IBlobStorageService
{
    Task<BoardDto?> DownloadJsonAsync(int boardId);
    
    Task DeleteBlobAsync(int boardId);
}