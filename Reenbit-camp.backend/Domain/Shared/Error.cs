namespace Domain.Shared;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get;}
    
    public string Message { get;}

    public static implicit operator string(Error error) => error.Code;
    public bool Equals(Error? other)
    {
        if (other == null)
        {
            return false;
        }
        return Code == other.Code && Message == other.Message;
    }
    
    public static bool operator ==(Error? first, Error? second)
    {
        if (first is null && second is null)
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        return first.Equals(second);
    }

    public static bool operator !=(Error? first, Error? second) => !(first == second);
    
    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => Code.GetHashCode();

    public override string ToString() => Code;
}