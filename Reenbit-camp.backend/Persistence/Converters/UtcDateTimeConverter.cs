using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Converters;

public static class UtcDateTimeConverters
{
    public static readonly ValueConverter<DateTime?, DateTime?> Nullable =
        new(
            v => v.HasValue
                ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                : v,
            v => v
        );

    public static readonly ValueConverter<DateTime, DateTime> NonNullable =
        new(
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
            v => v
        );
}