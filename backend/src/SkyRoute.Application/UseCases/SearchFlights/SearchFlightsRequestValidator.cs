using FluentValidation;
using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.UseCases.SearchFlights;

public class SearchFlightsRequestValidator : AbstractValidator<SearchFlightsRequest>
{
    public SearchFlightsRequestValidator()
    {
        RuleFor(x => x.OriginAirportCode)
            .NotEmpty()
            .Length(3);

        RuleFor(x => x.DestinationAirportCode)
            .NotEmpty()
            .Length(3)
            .NotEqual(x => x.OriginAirportCode)
            .WithMessage("Destination must be different from origin");

        RuleFor(x => x.DepartureDate)
            .Must(BeTodayOrFuture)
            .WithMessage("Departure date must be today or in the future");

        RuleFor(x => x.Passengers)
            .InclusiveBetween(1, 9);

        RuleFor(x => x.CabinClass)
            .Must(BeValidCabinClass)
            .WithMessage("CabinClass must be one of: Economy, Business, FirstClass");
    }

    private bool BeTodayOrFuture(DateTime date)
    {
        return date.Date >= DateTime.UtcNow.Date.AddDays(-1);
    }

    private bool BeValidCabinClass(string cabinClass)
    {
        return Enum.TryParse<CabinClass>(cabinClass, ignoreCase: false, out _);
    }
}
