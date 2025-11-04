namespace ByWay.Domain.Interfaces.Service
{
  public interface IEmailService
  {
    Task SendEmailAsync(string recipientName, string toEmail, string subject, string body);
  }
}
