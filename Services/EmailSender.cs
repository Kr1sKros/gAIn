using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace gain.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
        ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        string name = SetName(message); // Do before formatting message
        message = FormatMessage(message);
        
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("gain.app.contacts@gmail.com", name),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode 
            ? $"Email to {toEmail} queued successfully!"
            : $"Failure Email to {toEmail}");
    }

    private string SetName(string message)
    { // Do before formatting message
        
        // Reset password string: Please reset your password by clicking here.
        if (message.Contains("reset your password"))
        {
            return "Password Recovery";
        }
        // Confirm email string: Please confirm your account by clicking here.
        if (message.Contains("confirm your account"))
        {
            return "Email Confirmation";
        }
        return "gAIn Notification";
    }

    private string FormatMessage(string message)
    {
        string pre = "<!DOCTYPE html>\n<html lang=\"en\">\n  <head>\n    <meta charset=\"UTF-8\" />\n  </head>\n  <body>\n    <div\n      style=\"\n        position: relative;\n        width: 70vh;\n        height: 20vh;\n        background-image: url('https://i.ibb.co/KsNQLdx/asdfghjkl.png');\n        background-size: 100%;\n        background-size: contain;\n        background-position: center;\n        background-repeat: no-repeat;\n        background-position: top;\n        color: #66fcb6;\n      \"\n    >\n      <h3 style=\"margin: 15px\">\n        <span\n          style=\"\n            background-color: rgba(0, 0, 0, 0.6);\n            font-family: 'Courier New', Courier, monospace;\n          \"\n        >\n          &nbsp;";
        string post = "&nbsp;\n        </span>\n      </h3>\n    </div>\n  </body>\n</html>";
        
        return pre + message + post;
    }
}