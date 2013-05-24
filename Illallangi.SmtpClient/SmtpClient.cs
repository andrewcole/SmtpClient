using System.Net.Mail;
using System.Text;
using Ninject.Extensions.Logging;

namespace Illallangi
{
    public sealed class SmtpClient : ISmtpClient
    {
        #region Fields

        private readonly ILogger currentLogger;
        private System.Net.Mail.SmtpClient currentMailClient;

        #endregion Fields

        #region Constructor

        public SmtpClient(ILogger logger)
        {
            this.currentLogger = logger;
            this.Logger.Debug("Constructor Complete");
        }

        #endregion

        #region Methods

        public void SendEmail(string fromAddress, string toAddress, string subject, string body, bool isBodyHtml = true, Encoding bodyEncoding = null)
        {
            this.SendEmail(new MailMessage(fromAddress, toAddress, subject, body)
                {
                    IsBodyHtml = isBodyHtml,
                    BodyEncoding = bodyEncoding ?? Encoding.UTF8,
                });
        }

        public void SendEmail(MailMessage message)
        {
            this.Logger.Debug("Sending email {0} to {1} from {2} ({3} bytes)",
                message.Subject,
                message.To,
                message.From,
                message.Body.Length);

            try
            {
                this.MailClient.Send(message);
                this.Logger.Debug("Sent");
            }
            catch (SmtpFailedRecipientsException e)
            {
                this.Logger.ErrorException("Failed sending email to some recipients", e);
            }
            catch (SmtpException e)
            {
                this.Logger.ErrorException("Failed sending email", e);
            }
        }

        #endregion Methods

        #region Properties

        private ILogger Logger
        {
            get { return this.currentLogger; }
        }

        private System.Net.Mail.SmtpClient MailClient
        {
            get { return this.currentMailClient ?? (this.currentMailClient = new System.Net.Mail.SmtpClient()); }
        }

        #endregion
    }
}