namespace SkyRoute.Application.DTOs;

public record FlightResultDto(
    string FlightId,
    string Provider,
    string FlightNumber,
    string OriginCode,
    string DestinationCode,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    int DurationMinutes,
    string CabinClass,
    MoneyDto PricePerPassenger,
    MoneyDto TotalPrice,
    int PassengerCount);
