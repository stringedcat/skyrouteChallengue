using SkyRoute.Domain.Entities;

namespace SkyRoute.Application.Abstractions;

public interface IFlightProvider
{
    string ProviderName { get; }
    Task<IReadOnlyList<Flight>> SearchAsync(SearchCriteria criteria, CancellationToken ct);
}
