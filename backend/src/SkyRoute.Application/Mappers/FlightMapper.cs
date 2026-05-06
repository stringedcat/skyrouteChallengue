using SkyRoute.Application.DTOs;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Application.Mappers;

public static class FlightMapper
{
    public static FlightResultDto ToDto(Flight flight, Money pricePerPassenger, int passengerCount)
    {
        var totalPrice = pricePerPassenger.Multiply(passengerCount);

        return new FlightResultDto(
            FlightId: flight.FlightId,
            Provider: flight.Provider,
            FlightNumber: flight.FlightNumber,
            OriginCode: flight.Origin.Code,
            DestinationCode: flight.Destination.Code,
            DepartureTime: flight.DepartureTime,
            ArrivalTime: flight.ArrivalTime,
            DurationMinutes: flight.DurationMinutes,
            CabinClass: flight.CabinClass.ToString(),
            PricePerPassenger: new MoneyDto(pricePerPassenger.Amount, pricePerPassenger.Currency),
            TotalPrice: new MoneyDto(totalPrice.Amount, totalPrice.Currency),
            PassengerCount: passengerCount);
    }
}
