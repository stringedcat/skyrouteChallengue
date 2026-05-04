namespace SkyRoute.Application.DTOs;

public record SearchFlightsResponse(
    bool IsInternational,
    string RequiredDocument,
    IReadOnlyList<FlightResultDto> Results);
