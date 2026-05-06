using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Abstractions;

public record SearchCriteria(
    string OriginAirportCode,
    string DestinationAirportCode,
    DateTime DepartureDate,
    int Passengers,
    CabinClass CabinClass);
