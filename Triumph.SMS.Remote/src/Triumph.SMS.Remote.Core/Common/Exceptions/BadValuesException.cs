namespace Triumph.SMS.Remote.Core.Common.Exceptions;

public class BadValuesException : Exception
{
    public BadValuesException()
    {
    }
    public BadValuesException(string? message)
        : base(message)
    {
    }
    public BadValuesException(string? message, Exception? inner)
        : base(message, inner)
    {
    }
}
