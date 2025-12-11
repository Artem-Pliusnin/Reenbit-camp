namespace Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, ApiError? error)
    {
        if (isSuccess && error != null)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == null)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }
    
    public bool IsSuccess { get; }
    public ApiError? Error { get; }
    
    public bool IsFailure => !IsSuccess;
    
    public static Result Success() => new Result(true, null);
    
    public static Result<TValue> Success<TValue>(TValue value) => 
        new Result<TValue>(value, true, null);
    
    public static Result Failure(ApiError error) => 
        new Result(false, error);
    
    public static Result<TValue> Failure<TValue>(ApiError error) => 
        new Result<TValue>(default, false, error);
    
    public static Result<TValue> Create<TValue>(TValue? value) => 
        value != null 
            ? Success(value) 
            : Failure<TValue>(ApiError.NullValue);
}