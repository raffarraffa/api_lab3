using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace api.Services
{
    public class SendMailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPass;

        public SendMailService(IConfiguration configuration)
        {
            smtpServer = "smtp.gmail.com";
            smtpPort = 587;
            smtpUser = "lopezrafa@gmail.com";
            smtpPass = "xxxxxxxxxxxx";
        }

        public void SendMail(string toEmail, string subject, string body)
        {
            // await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            // client.Authenticate(smtpUser, smtpPass);
            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true; // Habilitar SSL

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUser),
                    Subject = subject,
                    Body = body,
                    //IsBodyHtml = false,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                try
                {
                    smtpClient.Send(mailMessage); // Enviar el correo
                }
                catch (Exception ex)
                {
                    // Manejar excepciones de envío
                    Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                }
            }
        }
    }
}
