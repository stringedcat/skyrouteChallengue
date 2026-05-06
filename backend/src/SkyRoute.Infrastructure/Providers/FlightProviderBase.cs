using SkyRoute.Application.Abstractions;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;
using SkyRoute.Domain.ValueObjects;
using SkyRoute.Infrastructure.Providers.MockData;

namespace SkyRoute.Infrastructure.Providers;

public abstract class FlightProviderBase : IFlightProvider
{
    private readonly IAirportRepository _airportRepository;

    public abstract string ProviderName { get; }
    protected abstract IEnumerable<MockFlightTemplate> Templates { get; }

    protected FlightProviderBase(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }

    public async Task<IReadOnlyList<Flight>> SearchAsync(SearchCriteria criteria, CancellationToken ct)
    {
        var origin = await _airportRepository.GetByCodeAsync(criteria.OriginAirportCode, ct);
        var destination = await _airportRepository.GetByCodeAsync(criteria.DestinationAirportCode, ct);

        if (origin is null || destination is null)
            return Array.Empty<Flight>();

        var flights = new List<Flight>();
        var dateString = criteria.DepartureDate.ToString("yyyyMMdd");

        foreach (var template in Templates)
        {
            var baseFare = SelectFare(template, criteria.CabinClass);
            var departureTime = DateTime.SpecifyKind(
                criteria.DepartureDate.Date.AddHours(template.DepartureHourUtc),
                DateTimeKind.Utc);
            var arrivalTime = departureTime.AddHours(template.DurationHours);

            var flight = new Flight(
                flightId: $"{template.FlightNumberPrefix}-{template.FlightNumberSeed}-{dateString}",
                provider: ProviderName,
                flightNumber: $"{template.FlightNumberPrefix}{template.FlightNumberSeed}",
                origin: origin,
                destination: destination,
                departureTime: departureTime,
                arrivalTime: arrivalTime,
                cabinClass: criteria.CabinClass,
                baseFare: new Money(baseFare));

            flights.Add(flight);
        }

        return flights;
    }

    private static decimal SelectFare(MockFlightTemplate template, CabinClass cabinClass)
        => cabinClass switch
        {
            CabinClass.Economy => template.BaseFareEconomy,
            CabinClass.Business => template.BaseFareBusiness,
            CabinClass.FirstClass => template.BaseFareFirstClass,
            _ => throw new ArgumentOutOfRangeException(nameof(cabinClass))
        };
}
