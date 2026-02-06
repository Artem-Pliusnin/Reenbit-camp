namespace Restoration.Function.Models.Enums;

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
    SentToRestorationServiceBusQueue = 9,
    Restored = 10,
    GotDataFromBlobStorage = 11,
    SavedToDataBase = 12,
    DeletedFromBlobStorage = 13,
    FailedToGetFromBlobStorage = 14,
    FailedToSaveToDataBase = 15,
    FailedToDeleteFromBlobStorage = 16,
}