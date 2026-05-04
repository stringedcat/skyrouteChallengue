using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Abstractions;

public interface IAirportRepository
{
    Task<IReadOnlyList<Airport>> GetAllAsync(CancellationToken ct);
    Task<Airport?> GetByCodeAsync(string code, CancellationToken ct);
}
