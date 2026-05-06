using SkyRoute.Domain.Enums;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Domain.Entities;

public class Flight
{
    public string FlightId { get; private set; } = null!;
    public string Provider { get; private set; } = null!;
    public string FlightNumber { get; private set; } = null!;
    public Airport Origin { get; private set; } = null!;
    public Airport Destination { get; private set; } = null!;
    public DateTime DepartureTime { get; private set; }
    public DateTime ArrivalTime { get; private set; }
    public CabinClass CabinClass { get; private set; }
    public Money BaseFare { get; private set; } = null!;

    public bool IsInternational => Origin.Country != Destination.Country;
    public DocumentType RequiredDocument => IsInternational ? DocumentType.Passport : DocumentType.NationalId;
    public int DurationMinutes => (int)(ArrivalTime - DepartureTime).TotalMinutes;

    private Flight() { }

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
