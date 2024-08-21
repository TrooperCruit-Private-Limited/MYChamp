using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailSenderService
{
    private readonly IConfiguration _configuration;

    public EmailSenderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("Smtp");
        var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"]))
        {
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["Username"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        try
        {
            await client.SendMailAsync(mailMessage);
        }
        catch (SmtpException ex)
        {
            // Log the exception or handle it as needed
            throw;
        }
    }
}
