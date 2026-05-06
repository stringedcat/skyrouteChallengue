using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Infrastructure.Providers.Options;

public class GlobalAirOptions
{
    public const string SectionName = "Providers:GlobalAir";

    [Range(0, 1, ErrorMessage = "FuelSurchargePercent must be between 0 and 1 (e.g. 0.15 for 15%).")]
    public decimal FuelSurchargePercent { get; set; } = 0.15m;
}
