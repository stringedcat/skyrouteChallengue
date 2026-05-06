using SkyRoute.Domain.Entities;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Application.Abstractions;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken ct);
    Task<Booking?> GetByReferenceAsync(BookingReference reference, CancellationToken ct);
}
