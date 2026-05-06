using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyRoute.Application.Abstractions;
using SkyRoute.Infrastructure.Persistence;
using SkyRoute.Infrastructure.Persistence.Repositories;
using SkyRoute.Infrastructure.PricingStrategies;
using SkyRoute.Infrastructure.Providers;
using SkyRoute.Infrastructure.Providers.Options;
using SkyRoute.Infrastructure.Services;

namespace SkyRoute.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SkyRouteDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("SkyRouteDb")
                ?? throw new InvalidOperationException("Connection string 'SkyRouteDb' not found")));

        services.AddOptions<GlobalAirOptions>()
            .Bind(configuration.GetSection(GlobalAirOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<BudgetWingsOptions>()
            .Bind(configuration.GetSection(BudgetWingsOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IFlightProvider, GlobalAirProvider>();
        services.AddScoped<IFlightProvider, BudgetWingsProvider>();

        services.AddScoped<IPricingStrategy, GlobalAirPricingStrategy>();
        services.AddScoped<IPricingStrategy, BudgetWingsPricingStrategy>();

        services.AddSingleton<IAirportRepository, AirportRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        services.AddSingleton<IBookingReferenceGenerator, BookingReferenceGenerator>();

        return services;
    }
}
