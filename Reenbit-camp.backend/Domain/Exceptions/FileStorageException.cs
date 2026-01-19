namespace Domain.Exceptions;

public class FileStorageException : Exception
{
    public FileStorageException(string message, Exception? innerException = null) 
        : base(message, innerException) { }
}