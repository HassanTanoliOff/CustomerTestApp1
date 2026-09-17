using System.Net;
using System.Net.Mail;
using CustomerTestApp1.Services.Interfaces;

namespace CustomerTestApp1.Services;

public class SmtpEmailService(IConfiguration config) : IEmailService {
  public async Task SendEmailAsync(string toEmail, string subject, string body) {
    var smtpHost = config["EmailSettings:SmtpHost"];
    var smtpPort = int.Parse(config["EmailSettings:SmtpPort"]?? "588");
    var senderEmail = config["EmailSettings:SenderEmail"];
    var senderPassword = config["EmailSettings:SenderPassword"];

    using var client = new SmtpClient(smtpHost, smtpPort);
    client.Credentials = new NetworkCredential(senderEmail, senderPassword);
    client.EnableSsl = true;

    var mailMessage = new MailMessage {
      From = new MailAddress(senderEmail),
      Subject = subject,
      Body = body,
      IsBodyHtml = false
    };

    mailMessage.To.Add(toEmail);

    await client.SendMailAsync(mailMessage);
    Console.WriteLine($"<><><>Host: {smtpHost}, Port: {smtpPort}, From: {senderEmail}<><><><>");
  }
}