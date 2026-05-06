using Microsoft.Extensions.Logging;
using SkyRoute.Application.Abstractions;
using SkyRoute.Application.Common;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.Mappers;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Application.UseCases.GetBookingByReference;

public class GetBookingByReferenceUseCase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<GetBookingByReferenceUseCase> _logger;

    public GetBookingByReferenceUseCase(
        IBookingRepository bookingRepository,
        ILogger<GetBookingByReferenceUseCase> logger)
    {
        _bookingRepository = bookingRepository;
        _logger = logger;
    }

    public async Task<Result<BookingConfirmationDto>> ExecuteAsync(string referenceString, CancellationToken ct)
    {
        BookingReference reference;
        try
        {
            reference = BookingReference.FromString(referenceString);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid booking reference format: {Reference}", referenceString);
            return Result<BookingConfirmationDto>.Failure(
                Error.Validation("INVALID_REFERENCE", ex.Message));
        }

        var booking = await _bookingRepository.GetByReferenceAsync(reference, ct);

        if (booking == null)
        {
            _logger.LogWarning("Booking not found: {Reference}", referenceString);
            return Result<BookingConfirmationDto>.Failure(
                Error.NotFound("BOOKING_NOT_FOUND", $"Booking with reference {referenceString} not found"));
        }

        _logger.LogInformation("Retrieved booking: {Reference}", referenceString);

        var dto = BookingMapper.ToConfirmationDto(booking);
        return Result<BookingConfirmationDto>.Success(dto);
    }
}
