using ByWay.Domain.Interfaces.Service;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace ByWay.Application.Services
{
  public class EmailService : IEmailService
  {
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
      _configuration = configuration;

    }

    public async Task SendEmailAsync(string recipientName, string toEmail, string subject, string body)
    {
      try
      {
        var smtpSettings = _configuration.GetSection("SmtpSettings");
        var server = smtpSettings["SmtpServer"];
        var port = int.Parse(smtpSettings["SmtpPort"]!);
        var isSslEnabled = bool.Parse(smtpSettings["EnableSsl"]!);
        var username = smtpSettings["Username"];
        var password = smtpSettings["Password"];

        var email = new MimeMessage();
        var from = new MailboxAddress("byway team", "noreply@gmail.com");
        email.From.Add(from);
        var to = new MailboxAddress(recipientName, toEmail);
        email.To.Add(to);
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Plain) { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(server, port, isSslEnabled);
        await client.AuthenticateAsync(username, password);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
