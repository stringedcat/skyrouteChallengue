using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Application.Abstractions;

public interface IPricingStrategy
{
    string ProviderName { get; }
    Money CalculateFinalPrice(Money baseFare);
}
