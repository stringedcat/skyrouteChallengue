using FluentValidation;
using SkyRoute.Application.DTOs;

namespace SkyRoute.Application.UseCases.CreateBooking;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.FlightId)
            .NotEmpty();

        RuleFor(x => x.Provider)
            .NotEmpty();

        RuleFor(x => x.DepartureDate)
            .Must(BeTodayOrFuture)
            .WithMessage("Departure date must be today or in the future");

        RuleFor(x => x.OriginAirportCode)
            .NotEmpty()
            .Length(3);

        RuleFor(x => x.DestinationAirportCode)
            .NotEmpty()
            .Length(3)
            .NotEqual(x => x.OriginAirportCode)
            .WithMessage("Destination must be different from origin");

        RuleFor(x => x.CabinClass)
            .Must(BeValidCabinClass)
            .WithMessage("CabinClass must be one of: Economy, Business, FirstClass");

        RuleFor(x => x.Passengers)
            .NotEmpty()
            .Must(passengers => passengers.Count <= 9)
            .WithMessage("Passengers count must be between 1 and 9");

        RuleForEach(x => x.Passengers)
            .ChildRules(passenger =>
            {
                passenger.RuleFor(p => p.FullName)
                    .NotEmpty()
                    .Length(3, 100);

                passenger.RuleFor(p => p.Email)
                    .NotEmpty()
                    .EmailAddress();

                passenger.RuleFor(p => p.DocumentNumber)
                    .NotEmpty()
                    .MaximumLength(20);
            });
    }

    private bool BeTodayOrFuture(DateTime date)
    {
        return date.Date >= DateTime.UtcNow.Date;
    }

    private bool BeValidCabinClass(string cabinClass)
    {
        return cabinClass is "Economy" or "Business" or "FirstClass";
    }
}
