using Common.Notification.Models;

namespace Common.Notification.Interfaces;

public interface IEmailServiceSender
{
    public Task SendEmailAsync(SendEmailArgs args, CancellationToken cancellationToken = default);
}