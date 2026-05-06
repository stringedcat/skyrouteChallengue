using Microsoft.EntityFrameworkCore;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Persistence;

public class SkyRouteDbContext : DbContext
{
    public DbSet<Booking> Bookings => Set<Booking>();

    public SkyRouteDbContext(DbContextOptions<SkyRouteDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkyRouteDbContext).Assembly);
    }
}
