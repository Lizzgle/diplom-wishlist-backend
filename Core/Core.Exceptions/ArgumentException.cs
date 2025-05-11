namespace Core.Exceptions;

public class ArgumentException : Exception
{
    private const string DefaultMessage = "Argument {0} is invalid";

    public ArgumentException()
        : base($"{DefaultMessage}.") { }

    public ArgumentException(string message)
        : base($"{DefaultMessage}. {message}") { }
}