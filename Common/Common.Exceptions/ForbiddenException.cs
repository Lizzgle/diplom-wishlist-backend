namespace Common.Exceptions;

public class ForbiddenException : Exception
{
    private const string DefaultMessage = "You have not access to this action.";

    public ForbiddenException()
        : base($"{DefaultMessage}.") { }

    public ForbiddenException(string message)
        : base($"{DefaultMessage}. {message}") { }
}