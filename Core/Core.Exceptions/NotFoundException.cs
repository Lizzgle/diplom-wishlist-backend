namespace Core.Exceptions;

public class NotFoundException : Exception
{
    private const string DefaultMessage = "Not found entity";

    public NotFoundException()
        : base($"{DefaultMessage}.") { }

    public NotFoundException(string message)
        : base($"{DefaultMessage}. {message}") { }
}