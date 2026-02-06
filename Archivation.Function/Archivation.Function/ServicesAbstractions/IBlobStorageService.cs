using Archivation.Function.Models.DTos;

namespace Archivation.Function.ServicesAbstractions;

public interface IBlobStorageService
{
    Task UploadJsonAsync(string fileName, BoardDto board);
}