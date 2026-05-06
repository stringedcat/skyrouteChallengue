namespace SkyRoute.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrEmpty(currency))
            throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

        Amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        Currency = currency;
    }

    public Money Multiply(int factor)
    {
        return new Money(Amount * factor, Currency);
    }

    public Money ApplyPercentage(decimal percent)
    {
        var multiplier = 1m + percent;
        return new Money(Amount * multiplier, Currency);
    }

    public Money EnsureMinimum(decimal minimum)
    {
        return new Money(Math.Max(Amount, minimum), Currency);
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot add money with different currencies: {left.Currency} and {right.Currency}");

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money Zero => new Money(0m, "USD");
}
