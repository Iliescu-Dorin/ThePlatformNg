namespace Core.SharedKernel.Exceptions;

public class TrnInvalidException : Exception
{
    public TrnInvalidException(string trn)
        : base($"TRN format \"{trn}\" is invalid")
    {
    }
}
