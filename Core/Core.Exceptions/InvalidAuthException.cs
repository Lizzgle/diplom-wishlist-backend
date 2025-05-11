namespace Core.Exceptions;

public class InvalidAuthException : Exception
{
    private const string DefaultMessage = "Invalid credentials";

    public InvalidAuthException()
        : base($"{DefaultMessage}.") { }

    public InvalidAuthException(string message)
        : base($"{DefaultMessage}. {message}") { }
}