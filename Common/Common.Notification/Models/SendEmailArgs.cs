namespace Common.Notification.Models;

public class SendEmailArgs
{
    public string Email { get; set; } = null!;
    
    public string Subject { get; set; } = null!;
    
    public string Message {get; set;} = null!;
}