namespace ByWay.Application.Exceptions;

public class CheckoutValidationException : Exception
{
    public CheckoutValidationException(string message) : base(message)
    {
    }
}