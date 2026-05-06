namespace SkyRoute.Infrastructure.Providers.MockData;

public record MockFlightTemplate(
    string FlightNumberPrefix,
    int FlightNumberSeed,
    int DepartureHourUtc,
    int DurationHours,
    decimal BaseFareEconomy,
    decimal BaseFareBusiness,
    decimal BaseFareFirstClass);
