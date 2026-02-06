using Restoration.Function.Models.Enums;

namespace Restoration.Function.ServicesAbstractions;

public interface IArchivationLogsService
{
    Task SaveArchivationLogAsync(int boardId, ArchiveStatus status);
}