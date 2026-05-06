using SkyRoute.Application.Abstractions;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Infrastructure.Services;

public class BookingReferenceGenerator : IBookingReferenceGenerator
{
    public BookingReference Generate() => BookingReference.Generate();
}
