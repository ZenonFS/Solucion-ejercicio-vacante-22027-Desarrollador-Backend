using MailKit.Security;
using MimeKit;
    
namespace TravelAgencyApp.Utils
{
    public class Functions
    {
        public void SendEmail(string emailAddress, string subject, string body)
        {
            // Replace with your Gmail credentials and email address
            string userName = "santiago.suarez6135@gmail.com";
            string password = "znsmxzpuhvqxkgie";

            // Create a new message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Santiago Suárez", userName));
            message.To.Add(new MailboxAddress(emailAddress.Split("@")[0], emailAddress));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            // Configure the SMTP client
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(userName, password);

                // Send the email message
                client.Send(message);

                client.Disconnect(true);
            }
        }
    }
}



