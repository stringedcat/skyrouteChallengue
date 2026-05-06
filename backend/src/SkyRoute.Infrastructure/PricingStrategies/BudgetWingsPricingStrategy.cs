using Microsoft.Extensions.Options;
using SkyRoute.Application.Abstractions;
using SkyRoute.Domain.ValueObjects;
using SkyRoute.Infrastructure.Providers.Options;

namespace SkyRoute.Infrastructure.PricingStrategies;

public class BudgetWingsPricingStrategy : IPricingStrategy
{
    private readonly BudgetWingsOptions _options;

    public string ProviderName => "BudgetWings";

    public BudgetWingsPricingStrategy(IOptions<BudgetWingsOptions> options)
    {
        _options = options.Value;
    }

    public Money CalculateFinalPrice(Money baseFare)
    {
        var discounted = baseFare.ApplyPercentage(-_options.DiscountPercent);
        return discounted.EnsureMinimum(_options.MinimumFare);
    }
}
