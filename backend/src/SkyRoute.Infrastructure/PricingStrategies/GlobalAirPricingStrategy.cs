using Microsoft.Extensions.Options;
using SkyRoute.Application.Abstractions;
using SkyRoute.Domain.ValueObjects;
using SkyRoute.Infrastructure.Providers.Options;

namespace SkyRoute.Infrastructure.PricingStrategies;

public class GlobalAirPricingStrategy : IPricingStrategy
{
    private readonly GlobalAirOptions _options;

    public string ProviderName => "GlobalAir";

    public GlobalAirPricingStrategy(IOptions<GlobalAirOptions> options)
    {
        _options = options.Value;
    }

    public Money CalculateFinalPrice(Money baseFare)
    {
        return baseFare.ApplyPercentage(_options.FuelSurchargePercent);
    }
}
