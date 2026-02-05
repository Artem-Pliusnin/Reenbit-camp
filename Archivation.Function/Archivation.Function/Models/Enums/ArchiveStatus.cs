namespace Archivation.Function.Models.Enums;

public enum ArchiveStatus
{
    MarkedAsPending = 1,
    SentToServiceBusQueue = 2,
    GotFromDatabase = 3,
    SavedToBlobStorage = 4,
    DeletedFromDataBase = 5,
    FailedToGetFromDataBase = 6,
    FailedToDeleteFromDataBase = 7,
    FailedToSaveToBlobStorage = 8,
}