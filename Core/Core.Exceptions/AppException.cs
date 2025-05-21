namespace Core.Exceptions;

public class AppException : Exception
{
    private const string DefaultMessage = "App exception";

    public AppException()
        : base($"{DefaultMessage}.") { }

    public AppException(string message)
        : base($"{DefaultMessage}. {message}") { }
}