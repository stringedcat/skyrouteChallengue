namespace SkyRoute.Application.DTOs;

public record PassengerSummaryDto(string FullName, string Email);

public record PriceBreakdownDto(
    MoneyDto PricePerPassenger,
    int PassengerCount,
    MoneyDto TotalPrice);

public record BookingConfirmationDto(
    string BookingReference,
    string Status,
    DateTime CreatedAt,
    FlightResultDto Flight,
    IReadOnlyList<PassengerSummaryDto> Passengers,
    PriceBreakdownDto PriceBreakdown);
