using Domain.Enums;

namespace Application.Abstractions.Services;

public interface IArchivationLogsService
{
    Task SaveArchivationLogAsync(int boardId, ArchiveStatus status);
}