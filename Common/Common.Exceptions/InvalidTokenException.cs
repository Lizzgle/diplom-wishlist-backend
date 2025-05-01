namespace Common.Exceptions;

public class InvalidTokenException : Exception
{
    private const string DefaultMessage = "Invalid token.";

    public InvalidTokenException()
        : base($"{DefaultMessage}.") { }

    public InvalidTokenException(string message)
        : base($"{DefaultMessage}. {message}") { }
}