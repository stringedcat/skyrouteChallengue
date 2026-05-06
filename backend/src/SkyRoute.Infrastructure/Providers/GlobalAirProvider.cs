using SkyRoute.Application.Abstractions;
using SkyRoute.Infrastructure.Providers.MockData;

namespace SkyRoute.Infrastructure.Providers;

public class GlobalAirProvider : FlightProviderBase
{
    public override string ProviderName => "GlobalAir";
    protected override IEnumerable<MockFlightTemplate> Templates => GlobalAirMockData.GetTemplates();

    public GlobalAirProvider(IAirportRepository airportRepository) : base(airportRepository)
    {
    }
}
