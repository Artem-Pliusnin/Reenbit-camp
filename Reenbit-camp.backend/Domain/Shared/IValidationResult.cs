namespace Domain.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new Error(
        "ValidationError",
        "A validation error has occurred.");
    
    Error[] Errors { get; }
}