namespace SkyRoute.Application.DTOs;

public record SearchFlightsRequest(
    string OriginAirportCode,
    string DestinationAirportCode,
    DateTime DepartureDate,
    int Passengers,
    string CabinClass);
