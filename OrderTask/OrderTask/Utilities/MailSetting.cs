using System.Net;
using System.Net.Mail;

namespace OrderTask.Utilities
{
    public class EMail
    {
        public string Subject { get; set; }
        public string body { get; set; }
        public string Recipient { get; set; }

    }
    public static class MailSetting
    {
        public static void SendMail(EMail mail)
        {
            // Create SMTP client
            var client = new SmtpClient("smtp.gmail.com",587); 
            client.EnableSsl = true;
            //Create Network Credentials
            client.Credentials = new NetworkCredential("mdx7700@gmail.com", "djbvfstqwjhdrdny");
            client.Send("mdx7700@gmail.com", mail.Recipient , mail.Subject , mail.body);

        }
    }
}
