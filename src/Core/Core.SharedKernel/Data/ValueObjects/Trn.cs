using Core.SharedKernel.Exceptions;
using ValueOf;

namespace Core.SharedKernel.Data.ValueObjects;

public class Trn : ValueOf<string, Trn>
{
    private const int RequiredLength = 8;

    protected override void Validate()
    {
        if (Value.Length != RequiredLength)
        {
            throw new TrnInvalidException(Value);
        }

        if (!int.TryParse(Value, out _))
        {
            throw new TrnInvalidException(Value);
        }
    }
}
