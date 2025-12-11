namespace Domain.Shared;

public class ApiError
{
    public string? Title { get; set; }
    public string? Type { get; set; }
    public string? Detail { get; set; }
    public int? Status { get; set; }
    
    public static ApiError NullValue => new()
    {
        Title = "Null Value",
        Type = "NullValue",
        Detail = "Specified value is null",
        Status = 400
    };
}