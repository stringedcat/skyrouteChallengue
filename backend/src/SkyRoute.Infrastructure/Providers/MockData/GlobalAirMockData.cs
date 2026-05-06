namespace SkyRoute.Infrastructure.Providers.MockData;

public static class GlobalAirMockData
{
    public static IEnumerable<MockFlightTemplate> GetTemplates() =>
    [
        new MockFlightTemplate("GA", 100, DepartureHourUtc: 6,  DurationHours: 2, BaseFareEconomy: 120.00m, BaseFareBusiness: 340.00m, BaseFareFirstClass: 680.00m),
        new MockFlightTemplate("GA", 200, DepartureHourUtc: 10, DurationHours: 3, BaseFareEconomy: 185.00m, BaseFareBusiness: 520.00m, BaseFareFirstClass: 1040.00m),
        new MockFlightTemplate("GA", 300, DepartureHourUtc: 14, DurationHours: 4, BaseFareEconomy: 250.00m, BaseFareBusiness: 710.00m, BaseFareFirstClass: 1420.00m),
        new MockFlightTemplate("GA", 400, DepartureHourUtc: 18, DurationHours: 3, BaseFareEconomy: 210.00m, BaseFareBusiness: 590.00m, BaseFareFirstClass: 1180.00m),
        new MockFlightTemplate("GA", 500, DepartureHourUtc: 22, DurationHours: 5, BaseFareEconomy: 295.00m, BaseFareBusiness: 830.00m, BaseFareFirstClass: 1660.00m),
    ];
}
