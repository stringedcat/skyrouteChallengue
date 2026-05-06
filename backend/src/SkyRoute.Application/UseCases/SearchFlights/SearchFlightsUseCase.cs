using Microsoft.Extensions.Logging;
using SkyRoute.Application.Abstractions;
using SkyRoute.Application.Common;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.Mappers;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.UseCases.SearchFlights;

public class SearchFlightsUseCase
{
    private readonly IEnumerable<IFlightProvider> _providers;
    private readonly IEnumerable<IPricingStrategy> _pricingStrategies;
    private readonly IAirportRepository _airportRepository;
    private readonly ILogger<SearchFlightsUseCase> _logger;

    public SearchFlightsUseCase(
        IEnumerable<IFlightProvider> providers,
        IEnumerable<IPricingStrategy> pricingStrategies,
        IAirportRepository airportRepository,
        ILogger<SearchFlightsUseCase> logger)
    {
        _providers = providers;
        _pricingStrategies = pricingStrategies;
        _airportRepository = airportRepository;
        _logger = logger;
    }

    public async Task<Result<SearchFlightsResponse>> ExecuteAsync(
        SearchFlightsRequest request,
        CancellationToken ct)
    {
        var origin = await _airportRepository.GetByCodeAsync(request.OriginAirportCode, ct);
        if (origin == null)
        {
            return Result<SearchFlightsResponse>.Failure(
                Error.NotFound("ORIGIN_NOT_FOUND", $"Origin airport {request.OriginAirportCode} not found"));
        }

        var destination = await _airportRepository.GetByCodeAsync(request.DestinationAirportCode, ct);
        if (destination == null)
        {
            return Result<SearchFlightsResponse>.Failure(
                Error.NotFound("DESTINATION_NOT_FOUND", $"Destination airport {request.DestinationAirportCode} not found"));
        }

        if (origin.Code == destination.Code)
        {
            return Result<SearchFlightsResponse>.Failure(
                Error.Validation("SAME_AIRPORT", "Origin and destination must be different"));
        }

        if (!Enum.TryParse<CabinClass>(request.CabinClass, out var cabinClass))
        {
            return Result<SearchFlightsResponse>.Failure(
                Error.Validation("INVALID_CABIN_CLASS", $"Invalid cabin class: {request.CabinClass}"));
        }

        var criteria = new SearchCriteria(
            request.OriginAirportCode,
            request.DestinationAirportCode,
            request.DepartureDate,
            request.Passengers,
            cabinClass);

        var isInternational = origin.Country != destination.Country;
        var requiredDocument = isInternational ? "Passport" : "NationalId";

        var searchTasks = _providers.Select(provider => SafeSearchAsync(provider, criteria, ct)).ToArray();
        var providerResults = await Task.WhenAll(searchTasks);

        var allFlights = providerResults.SelectMany(flights => flights).ToList();

        var flightDtos = new List<FlightResultDto>();

        foreach (var flight in allFlights)
        {
            var strategy = _pricingStrategies.FirstOrDefault(s => s.ProviderName == flight.Provider);

            if (strategy == null)
            {
                _logger.LogWarning("No pricing strategy found for provider {Provider}, skipping flight {FlightId}",
                    flight.Provider, flight.FlightId);
                continue;
            }

            var pricePerPassenger = strategy.CalculateFinalPrice(flight.BaseFare);
            var flightDto = FlightMapper.ToDto(flight, pricePerPassenger, request.Passengers);
            flightDtos.Add(flightDto);
        }

        _logger.LogInformation("Search completed: {FlightCount} flights found from {ProviderCount} providers",
            flightDtos.Count, _providers.Count());

        var response = new SearchFlightsResponse(isInternational, requiredDocument, flightDtos);
        return Result<SearchFlightsResponse>.Success(response);
    }

    private async Task<IReadOnlyList<Flight>> SafeSearchAsync(
        IFlightProvider provider,
        SearchCriteria criteria,
        CancellationToken ct)
    {
        try
        {
            return await provider.SearchAsync(criteria, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Provider {Provider} failed during search", provider.ProviderName);
            return Array.Empty<Flight>();
        }
    }
}
