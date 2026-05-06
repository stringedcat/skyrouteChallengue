using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SkyRoute.Application.UseCases.CreateBooking;
using SkyRoute.Application.UseCases.GetAirports;
using SkyRoute.Application.UseCases.GetBookingByReference;
using SkyRoute.Application.UseCases.SearchFlights;

namespace SkyRoute.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<SearchFlightsUseCase>();
        services.AddScoped<CreateBookingUseCase>();
        services.AddScoped<GetAirportsUseCase>();
        services.AddScoped<GetBookingByReferenceUseCase>();

        services.AddValidatorsFromAssemblyContaining<SearchFlightsRequestValidator>();

        return services;
    }
}
