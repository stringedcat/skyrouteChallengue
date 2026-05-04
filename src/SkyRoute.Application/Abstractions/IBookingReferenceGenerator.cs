using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Application.Abstractions;

public interface IBookingReferenceGenerator
{
    BookingReference Generate();
}
