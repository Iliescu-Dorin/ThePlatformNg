using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.Data.ValueObjects.Mass;
public record Mass
{
    public float Value { get; init; }

    [Required]
    public MassUnit Unit { get; init; }

    public Mass(float value, MassUnit unit)
    {
        if (value < 0)
            throw new ArgumentException("Value cannot be negative", nameof(value));

        Value = value;
        Unit = unit;
    }

    public Mass ConvertTo(MassUnit newUnit)
    {
        if (newUnit == Unit)
            return this;

        return new Mass(
            value: Value * Unit.ConversionRateToGram / newUnit.ConversionRateToGram,
            unit: newUnit
        );
    }

    public override string ToString()
        => $"{Value:n} {Unit.Symbol}";
}
