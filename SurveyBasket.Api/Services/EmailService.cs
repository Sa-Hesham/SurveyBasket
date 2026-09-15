using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace SurveyBasket.Api.Services;

public class EmailService(IOptions<EmailSetting>Emailsetting ,ILogger<EmailService> logger) : IEmailSender
{
    private readonly EmailSetting _Emailsetting = Emailsetting.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async  Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //sender 
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_Emailsetting.Email),
            Subject = subject,


        };

        // to 
        message.To.Add(MailboxAddress.Parse(email));

        // creat messagebody 
        var builder = new BodyBuilder
        {
            HtmlBody= htmlMessage,
        };

        message.Body = builder.ToMessageBody();


        using var smtp = new SmtpClient();
        _logger.LogInformation("Send Email to {Email}  ", email);

      await  smtp.ConnectAsync(_Emailsetting.Url, _Emailsetting.Port, SecureSocketOptions.StartTls);
       await  smtp.AuthenticateAsync (_Emailsetting.Email , _Emailsetting.Password);
        await smtp.SendAsync(message);
       await  smtp.DisconnectAsync (true);
    }
}
