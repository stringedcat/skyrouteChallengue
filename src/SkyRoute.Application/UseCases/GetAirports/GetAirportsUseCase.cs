using Microsoft.Extensions.Logging;
using SkyRoute.Application.Abstractions;
using SkyRoute.Application.Common;
using SkyRoute.Application.DTOs;
using SkyRoute.Application.Mappers;

namespace SkyRoute.Application.UseCases.GetAirports;

public class GetAirportsUseCase
{
    private readonly IAirportRepository _airportRepository;
    private readonly ILogger<GetAirportsUseCase> _logger;

    public GetAirportsUseCase(
        IAirportRepository airportRepository,
        ILogger<GetAirportsUseCase> logger)
    {
        _airportRepository = airportRepository;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<AirportDto>>> ExecuteAsync(CancellationToken ct)
    {
        var airports = await _airportRepository.GetAllAsync(ct);
        var airportDtos = airports.Select(AirportMapper.ToDto).ToList();

        _logger.LogInformation("Retrieved {Count} airports", airportDtos.Count);

        return Result<IReadOnlyList<AirportDto>>.Success(airportDtos);
    }
}
