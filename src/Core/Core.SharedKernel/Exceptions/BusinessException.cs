namespace Core.SharedKernel.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string errorCode) : base(errorCode)
    {
    }
}
