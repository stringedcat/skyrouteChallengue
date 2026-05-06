namespace SkyRoute.Domain.Entities;

public class Passenger
{
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string DocumentNumber { get; private set; } = null!;

    private Passenger() { }

    public Passenger(string fullName, string email, string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("FullName is required", nameof(fullName));

        if (fullName.Length < 3 || fullName.Length > 100)
            throw new ArgumentException("FullName must be between 3 and 100 characters", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new ArgumentException("DocumentNumber is required", nameof(documentNumber));

        FullName = fullName;
        Email = email;
        DocumentNumber = documentNumber;
    }
}
