using SkyRoute.Application.Abstractions;
using SkyRoute.Domain.Entities;
using SkyRoute.Infrastructure.Persistence.SeedData;

namespace SkyRoute.Infrastructure.Persistence.Repositories;

public class AirportRepository : IAirportRepository
{
    private readonly IReadOnlyList<Airport> _airports;
    private readonly Dictionary<string, Airport> _airportsByCode;

    public AirportRepository()
    {
        _airports = AirportSeedData.GetAll();
        _airportsByCode = _airports.ToDictionary(
            a => a.Code,
            a => a,
            StringComparer.OrdinalIgnoreCase);
    }

    public Task<IReadOnlyList<Airport>> GetAllAsync(CancellationToken ct)
    {
        return Task.FromResult(_airports);
    }

    public Task<Airport?> GetByCodeAsync(string code, CancellationToken ct)
    {
        _airportsByCode.TryGetValue(code, out var airport);
        return Task.FromResult(airport);
    }
}
