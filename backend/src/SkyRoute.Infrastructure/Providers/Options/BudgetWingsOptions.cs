using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Infrastructure.Providers.Options;

public class BudgetWingsOptions
{
    public const string SectionName = "Providers:BudgetWings";

    [Range(0, 1, ErrorMessage = "DiscountPercent must be between 0 and 1 (e.g. 0.10 for 10%).")]
    public decimal DiscountPercent { get; set; } = 0.10m;

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "MinimumFare must be a non-negative value.")]
    public decimal MinimumFare { get; set; } = 29.99m;
}
