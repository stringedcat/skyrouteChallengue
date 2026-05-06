namespace SkyRoute.Infrastructure.Providers.MockData;

public static class BudgetWingsMockData
{
    public static IEnumerable<MockFlightTemplate> GetTemplates() =>
    [
        new MockFlightTemplate("BW", 100, DepartureHourUtc: 5,  DurationHours: 2, BaseFareEconomy: 85.00m,  BaseFareBusiness: 240.00m, BaseFareFirstClass: 480.00m),
        new MockFlightTemplate("BW", 200, DepartureHourUtc: 9,  DurationHours: 3, BaseFareEconomy: 130.00m, BaseFareBusiness: 365.00m, BaseFareFirstClass: 730.00m),
        new MockFlightTemplate("BW", 300, DepartureHourUtc: 13, DurationHours: 4, BaseFareEconomy: 175.00m, BaseFareBusiness: 490.00m, BaseFareFirstClass: 980.00m),
        new MockFlightTemplate("BW", 400, DepartureHourUtc: 17, DurationHours: 3, BaseFareEconomy: 150.00m, BaseFareBusiness: 420.00m, BaseFareFirstClass: 840.00m),
        new MockFlightTemplate("BW", 500, DepartureHourUtc: 21, DurationHours: 5, BaseFareEconomy: 200.00m, BaseFareBusiness: 560.00m, BaseFareFirstClass: 1120.00m),
    ];
}
