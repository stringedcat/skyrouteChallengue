using Microsoft.EntityFrameworkCore;
using SkyRoute.Application.Abstractions;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Infrastructure.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly SkyRouteDbContext _context;

    public BookingRepository(SkyRouteDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Booking booking, CancellationToken ct)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Booking?> GetByReferenceAsync(BookingReference reference, CancellationToken ct)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(b => b.Reference == reference, ct);
    }
}
