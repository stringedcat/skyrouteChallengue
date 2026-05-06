using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Persistence.SeedData;

public static class AirportSeedData
{
    public static IReadOnlyList<Airport> GetAll() =>
    [
        new Airport
        {
            Code = "EZE",
            Name = "Ministro Pistarini International",
            City = "Buenos Aires",
            Country = "AR",
            CountryName = "Argentina"
        },
        new Airport
        {
            Code = "COR",
            Name = "Ingeniero Ambrosio Taravella",
            City = "Córdoba",
            Country = "AR",
            CountryName = "Argentina"
        },
        new Airport
        {
            Code = "MDZ",
            Name = "El Plumerillo",
            City = "Mendoza",
            Country = "AR",
            CountryName = "Argentina"
        },
        new Airport
        {
            Code = "JFK",
            Name = "John F. Kennedy International",
            City = "New York",
            Country = "US",
            CountryName = "United States"
        },
        new Airport
        {
            Code = "MIA",
            Name = "Miami International",
            City = "Miami",
            Country = "US",
            CountryName = "United States"
        },
        new Airport
        {
            Code = "LAX",
            Name = "Los Angeles International",
            City = "Los Angeles",
            Country = "US",
            CountryName = "United States"
        }
    ];
}
