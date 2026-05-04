using Microsoft.Extensions.Logging;
using SkyRoute.Application.Abstractions;
using SkyRoute.Application.Common;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.Mappers;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.UseCases.CreateBooking;

public class CreateBookingUseCase
{
    private readonly IEnumerable<IFlightProvider> _providers;
    private readonly IEnumerable<IPricingStrategy> _pricingStrategies;
    private readonly IAirportRepository _airportRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingReferenceGenerator _referenceGenerator;
    private readonly ILogger<CreateBookingUseCase> _logger;

    public CreateBookingUseCase(
        IEnumerable<IFlightProvider> providers,
        IEnumerable<IPricingStrategy> pricingStrategies,
        IAirportRepository airportRepository,
        IBookingRepository bookingRepository,
        IBookingReferenceGenerator referenceGenerator,
        ILogger<CreateBookingUseCase> logger)
    {
        _providers = providers;
        _pricingStrategies = pricingStrategies;
        _airportRepository = airportRepository;
        _bookingRepository = bookingRepository;
        _referenceGenerator = referenceGenerator;
        _logger = logger;
    }

    public async Task<Result<BookingConfirmationDto>> ExecuteAsync(
        CreateBookingRequest request,
        CancellationToken ct)
    {
        var origin = await _airportRepository.GetByCodeAsync(request.OriginAirportCode, ct);
        if (origin == null)
        {
            return Result<BookingConfirmationDto>.Failure(
                Error.NotFound("ORIGIN_NOT_FOUND", $"Origin airport {request.OriginAirportCode} not found"));
        }

        var destination = await _airportRepository.GetByCodeAsync(request.DestinationAirportCode, ct);
        if (destination == null)
        {
            return Result<BookingConfirmationDto>.Failure(
                Error.NotFound("DESTINATION_NOT_FOUND", $"Destination airport {request.DestinationAirportCode} not found"));
        }

        var provider = _providers.FirstOrDefault(p => p.ProviderName == request.Provider);
        if (provider == null)
        {
            return Result<BookingConfirmationDto>.Failure(
                Error.NotFound("PROVIDER_NOT_FOUND", $"Provider {request.Provider} not found"));
        }

        if (!Enum.TryParse<CabinClass>(request.CabinClass, out var cabinClass))
        {
            return Result<BookingConfirmationDto>.Failure(
                Error.Validation("INVALID_CABIN_CLASS", $"Invalid cabin class: {request.CabinClass}"));
        }

        var criteria = new SearchCriteria(
            request.OriginAirportCode,
            request.DestinationAirportCode,
            request.DepartureDate,
            request.Passengers.Count,
            cabinClass);

        IReadOnlyList<Flight> flights;
        try
        {
            flights = await provider.SearchAsync(criteria, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Provider {Provider} failed during booking search", provider.ProviderName);
            return Result<BookingConfirmationDto>.Failure(
                Error.ProviderFailure("PROVIDER_ERROR", "Flight provider failed to respond"));
        }

        var flight = flights.FirstOrDefault(f => f.FlightId == request.FlightId);
        if (flight == null)
        {
            return Result<BookingConfirmationDto>.Failure(
                Error.NotFound("FLIGHT_NOT_FOUND", $"Flight {request.FlightId} not found"));
        }

        var strategy = _pricingStrategies.FirstOrDefault(s => s.ProviderName == request.Provider);
        if (strategy == null)
        {
            _logger.LogError("No pricing strategy found for provider {Provider}", request.Provider);
            return Result<BookingConfirmationDto>.Failure(
                Error.Unexpected("STRATEGY_NOT_FOUND", "Pricing strategy not configured for this provider"));
        }

        var pricePerPassenger = strategy.CalculateFinalPrice(flight.BaseFare);
        var totalPrice = pricePerPassenger.Multiply(request.Passengers.Count);

        List<Passenger> passengers;
        try
        {
            passengers = request.Passengers
                .Select(p => new Passenger(p.FullName, p.Email, p.DocumentNumber))
                .ToList();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Passenger validation failed: {Message}", ex.Message);
            return Result<BookingConfirmationDto>.Failure(
                Error.Validation("INVALID_PASSENGER", ex.Message));
        }

        var reference = _referenceGenerator.Generate();

        Booking booking;
        try
        {
            booking = new Booking(
                reference,
                flight,
                passengers,
                pricePerPassenger,
                totalPrice,
                DateTime.UtcNow);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Booking creation failed: {Message}", ex.Message);
            return Result<BookingConfirmationDto>.Failure(
                Error.Validation("INVALID_BOOKING", ex.Message));
        }

        await _bookingRepository.AddAsync(booking, ct);

        _logger.LogInformation("Booking created: {Reference} for flight {FlightId}",
            reference, flight.FlightId);

        var confirmationDto = BookingMapper.ToConfirmationDto(booking);
        return Result<BookingConfirmationDto>.Success(confirmationDto);
    }
}
