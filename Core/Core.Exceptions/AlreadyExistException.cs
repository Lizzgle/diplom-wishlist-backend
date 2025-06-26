namespace Core.Exceptions;

public class AlreadyExistException : Exception
{
    private const string DefaultMessage = "Already exist entity";

    public AlreadyExistException()
        : base($"{DefaultMessage}.") { }

    public AlreadyExistException(string message)
        : base($"{DefaultMessage}. {message}") { }
}