using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.Data.ValueObjects.Money;
/// <summary>
/// Value object representing money. Simplified sample.
/// </summary>
public record Money
{
    public decimal Amount { get; init; }

    [Required]
    public Currency Currency { get; init; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0) throw new ArgumentException("Value cannot be negative.", nameof(amount));
        if (currency == null) throw new ArgumentNullException(nameof(currency));

        (Amount, Currency) = (amount, currency);
    }

    public Money Copy()
        => new Money(Amount, Currency);

    public Money AddAmount(decimal amount)
        => new Money(Amount + amount, Currency);

    public static Money operator +(Money left, Money right)
    {
        if (!Equals(left.Currency, right.Currency))
            throw new InvalidOperationException($"Mixing currencies is not supported. Cannot add money of '{left.Currency}' and money of '{right.Currency}'.");

        return left.AddAmount(right.Amount);
    }

    public static Money operator -(Money left, Money right)
    {
        if (!Equals(left.Currency, right.Currency))
            throw new InvalidOperationException($"Mixing currencies is not supported. Cannot subtract money of '{left.Currency}' and money of '{right.Currency}'.");

        return left.AddAmount(-right.Amount);
    }

    public static Money operator *(Money left, Money right)
    {
        if (!Equals(left.Currency, right.Currency))
            throw new InvalidOperationException("Multiplication of two money instances require both to have the same currency.");

        return new Money(left.Amount * right.Amount, left.Currency);
    }

    public static Money operator *(int scalar, Money money)
        => new Money(money.Amount * scalar, money.Currency);

    public static Money operator *(Money money, int scalar)
        => scalar * money;

    public static Money operator *(decimal scalar, Money money)
        => new Money(money.Amount * scalar, money.Currency);

    public static Money operator *(Money money, decimal scalar)
        => scalar * money;

    public override string ToString()
        => string.Format("{0}{1}{2:n}",
            Currency.SymbolWellKnown ? Currency.Symbol : Currency.Code,
            Currency.SymbolWellKnown ? null : " ",
            Amount);
}
