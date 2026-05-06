using SkyRoute.Application.Abstractions;
using SkyRoute.Infrastructure.Providers.MockData;

namespace SkyRoute.Infrastructure.Providers;

public class BudgetWingsProvider : FlightProviderBase
{
    public override string ProviderName => "BudgetWings";
    protected override IEnumerable<MockFlightTemplate> Templates => BudgetWingsMockData.GetTemplates();

    public BudgetWingsProvider(IAirportRepository airportRepository) : base(airportRepository)
    {
    }
}
