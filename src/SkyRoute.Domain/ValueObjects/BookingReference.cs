namespace SkyRoute.Domain.ValueObjects;

public record BookingReference
{
    private const string Prefix = "SR-";
    private const int AlphanumericLength = 8;
    private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string Value { get; init; }

    private BookingReference(string value)
    {
        Value = value;
    }

    public static BookingReference Generate()
    {
        var alphanumeric = new char[AlphanumericLength];

        for (int i = 0; i < AlphanumericLength; i++)
        {
            alphanumeric[i] = AllowedChars[Random.Shared.Next(AllowedChars.Length)];
        }

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var value = $"{Prefix}{new string(alphanumeric)}-{timestamp}";

        return new BookingReference(value);
    }

    public static BookingReference FromString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Booking reference cannot be null or empty", nameof(value));

        if (!value.StartsWith(Prefix))
            throw new ArgumentException($"Booking reference must start with '{Prefix}'", nameof(value));

        var parts = value.Substring(Prefix.Length).Split('-');

        if (parts.Length != 2)
            throw new ArgumentException("Invalid booking reference format", nameof(value));

        if (parts[0].Length != AlphanumericLength)
            throw new ArgumentException($"Alphanumeric part must be {AlphanumericLength} characters", nameof(value));

        if (!parts[0].All(c => AllowedChars.Contains(c)))
            throw new ArgumentException("Alphanumeric part contains invalid characters", nameof(value));

        if (!long.TryParse(parts[1], out _))
            throw new ArgumentException("Timestamp part must be numeric", nameof(value));

        return new BookingReference(value);
    }

    public override string ToString() => Value;
}
