using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Mappers;

public static class BookingMapper
{
    public static BookingConfirmationDto ToConfirmationDto(Booking booking)
    {
        var flightDto = FlightMapper.ToDto(
            booking.Flight,
            booking.PricePerPassenger,
            booking.Passengers.Count);

        var passengerSummaries = booking.Passengers
            .Select(p => new PassengerSummaryDto(p.FullName, p.Email))
            .ToList();

        var priceBreakdown = new PriceBreakdownDto(
            PricePerPassenger: new MoneyDto(booking.PricePerPassenger.Amount, booking.PricePerPassenger.Currency),
            PassengerCount: booking.Passengers.Count,
            TotalPrice: new MoneyDto(booking.TotalPrice.Amount, booking.TotalPrice.Currency));

        return new BookingConfirmationDto(
            BookingReference: booking.Reference.ToString(),
            Status: booking.Status.ToString(),
            CreatedAt: booking.CreatedAt,
            Flight: flightDto,
            Passengers: passengerSummaries,
            PriceBreakdown: priceBreakdown);
    }
}
