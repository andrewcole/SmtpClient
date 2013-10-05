using System.Net.Mail;
using System.Text;

namespace Illallangi
{
    public interface ISmtpClient
    {
        void SendEmail(MailMessage message);

        void SendEmail(string fromAddress, string toAddress, string subject, string body, bool isBodyHtml = true,
                       Encoding bodyEncoding = null);
    }
}