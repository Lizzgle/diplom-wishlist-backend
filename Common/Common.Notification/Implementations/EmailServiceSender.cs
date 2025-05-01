using System.Net;
using System.Reflection;
using Common.Notification.Interfaces;
using Common.Notification.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Common.Notification.Implementations;

public class EmailServiceSender : IEmailServiceSender
{
    private readonly ILogger<EmailServiceSender> _logger;

    public EmailServiceSender(ILogger<EmailServiceSender> logger)
    {
        _logger = logger;
    }
    
    public async Task SendEmailAsync(SendEmailArgs args, CancellationToken cancellationToken = default)
    {
        var mail = new MimeMessage();
        
        mail.From.Add(new MailboxAddress("Notification Service", "lizaveta21082003@gmail.com"));
        mail.To.Add(new MailboxAddress("", args.Email));
        mail.Subject = args.Subject;
        mail.Body = new TextPart("html") { Text = await GetEmailTemplateAsync(args.Message) };

        using var smtp = new SmtpClient();
        
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("lizaveta21082003@gmail.com", "ookzwvuchyeouecv");
        await smtp.SendAsync(mail);
        await smtp.DisconnectAsync(true);

        _logger.LogInformation($"Email to {args.Email} sent");
    }
    
    private async Task<string> GetEmailTemplateAsync(string callbackUrl)
    {
        var fileName = "EmailConfirmation.html";
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"Common.Notification.Templates.{fileName}";

        await using var stream = assembly.GetManifestResourceStream(resourceName)
                                 ?? throw new FileNotFoundException($"Resource {resourceName} not found.");
        using var reader = new StreamReader(stream);
        
        var template = reader.ReadToEnd();

        Console.WriteLine(callbackUrl);
        return template.Replace("{{ callbackUrl }}", WebUtility.HtmlEncode(callbackUrl));
    }
}