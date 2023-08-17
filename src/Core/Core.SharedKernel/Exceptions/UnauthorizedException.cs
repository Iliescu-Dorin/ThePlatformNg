namespace Core.SharedKernel.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) :
        base(message)
    {
    }
}
