using SkyRoute.Domain.Enums;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Domain.Entities;

public class Flight
{
    public string FlightId { get; init; }
    public string Provider { get; init; }
    public string FlightNumber { get; init; }
    public Airport Origin { get; init; }
    public Airport Destination { get; init; }
    public DateTime DepartureTime { get; init; }
    public DateTime ArrivalTime { get; init; }
    public CabinClass CabinClass { get; init; }
    public Money BaseFare { get; init; }

    public bool IsInternational => Origin.Country != Destination.Country;
    public DocumentType RequiredDocument => IsInternational ? DocumentType.Passport : DocumentType.NationalId;
    public int DurationMinutes => (int)(ArrivalTime - DepartureTime).TotalMinutes;

    public Flight(
        string flightId,
        string provider,
        string flightNumber,
        Airport origin,
        Airport destination,
        DateTime departureTime,
        DateTime arrivalTime,
        CabinClass cabinClass,
        Money baseFare)
    {
        if (arrivalTime <= departureTime)
            throw new ArgumentException("ArrivalTime must be after DepartureTime", nameof(arrivalTime));

        if (origin.Code == destination.Code)
            throw new ArgumentException("Origin and Destination must be different airports", nameof(destination));

        FlightId = flightId;
        Provider = provider;
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        CabinClass = cabinClass;
        BaseFare = baseFare;
    }
}
