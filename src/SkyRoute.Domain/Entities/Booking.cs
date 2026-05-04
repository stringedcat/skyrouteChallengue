using SkyRoute.Domain.Enums;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Domain.Entities;

public class Booking
{
    public BookingReference Reference { get; init; }
    public Flight Flight { get; init; }
    public IReadOnlyList<Passenger> Passengers { get; init; }
    public Money PricePerPassenger { get; init; }
    public Money TotalPrice { get; init; }
    public BookingStatus Status { get; private set; } = BookingStatus.Confirmed;
    public DateTime CreatedAt { get; init; }

    public Booking(
        BookingReference reference,
        Flight flight,
        IReadOnlyList<Passenger> passengers,
        Money pricePerPassenger,
        Money totalPrice,
        DateTime createdAt)
    {
        if (passengers == null || passengers.Count == 0)
            throw new ArgumentException("At least one passenger is required", nameof(passengers));

        if (passengers.Count > 9)
            throw new ArgumentException("Maximum 9 passengers allowed", nameof(passengers));

        var expectedTotal = pricePerPassenger.Multiply(passengers.Count);
        if (totalPrice.Amount != expectedTotal.Amount || totalPrice.Currency != expectedTotal.Currency)
            throw new ArgumentException("TotalPrice must equal PricePerPassenger multiplied by passenger count", nameof(totalPrice));

        ValidatePassengerDocuments(passengers, flight.RequiredDocument);

        Reference = reference;
        Flight = flight;
        Passengers = passengers;
        PricePerPassenger = pricePerPassenger;
        TotalPrice = totalPrice;
        CreatedAt = createdAt;
    }

    public void Cancel()
    {
        Status = BookingStatus.Cancelled;
    }

    private static void ValidatePassengerDocuments(IReadOnlyList<Passenger> passengers, DocumentType requiredType)
    {
        foreach (var passenger in passengers)
        {
            var documentNumber = passenger.DocumentNumber;

            if (requiredType == DocumentType.Passport)
            {
                if (!documentNumber.All(c => char.IsLetterOrDigit(c)))
                    throw new ArgumentException(
                        $"Passport number must contain only alphanumeric characters: {documentNumber}",
                        nameof(passengers));
            }
            else if (requiredType == DocumentType.NationalId)
            {
                if (!documentNumber.All(char.IsDigit))
                    throw new ArgumentException(
                        $"National ID must contain only digits: {documentNumber}",
                        nameof(passengers));
            }
        }
    }
}
