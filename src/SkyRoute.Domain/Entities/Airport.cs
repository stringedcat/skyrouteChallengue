namespace SkyRoute.Domain.Entities;

public record Airport
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public required string CountryName { get; init; }
}
