using Archivation.Function.Models.Enums;

namespace Archivation.Function.ServicesAbstractions;

public interface IArchivationLogsService
{
    Task SaveArchivationLogAsync(int boardId, ArchiveStatus status);
}