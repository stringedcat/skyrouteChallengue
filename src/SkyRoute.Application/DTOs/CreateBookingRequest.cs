namespace SkyRoute.Application.DTOs;

public record CreateBookingRequest(
    string FlightId,
    string Provider,
    DateTime DepartureDate,
    string OriginAirportCode,
    string DestinationAirportCode,
    string CabinClass,
    IReadOnlyList<PassengerDto> Passengers);
