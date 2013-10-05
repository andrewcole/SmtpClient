using System;
using System.Net.Mail;
using System.Text;
using Ninject.Extensions.Logging;

namespace Illallangi
{
    public sealed class SmtpClient : ISmtpClient
    {
        #region Fields

        private readonly ILogger currentLogger;
        private readonly ISmtpClientConfig currentConfig;
        private System.Net.Mail.SmtpClient currentMailClient;

        #endregion

        #region Constructors

        public SmtpClient()
            : this(null, null)
        {
        }

        public SmtpClient(ILogger logger)
            : this(null, logger)
        {
        }

        public SmtpClient(ISmtpClientConfig config)
            : this(config, null)
        {
        }

        public SmtpClient(ISmtpClientConfig config, ILogger logger)
        {
            this.currentLogger = logger ?? new NullLogger(typeof(SmtpClient));
            this.currentConfig = config ?? new NullSmtpClientConfig();
            this.Logger.Debug("Constructor Complete");
        }

        #endregion

        #region Methods

        public void SendEmail(string fromAddress, string toAddress, string subject, string body, bool isBodyHtml = true, Encoding bodyEncoding = null)
        {
            this.SendEmail(new System.Net.Mail.MailMessage(fromAddress, toAddress, subject, body)
                {
                    IsBodyHtml = isBodyHtml,
                    BodyEncoding = bodyEncoding ?? Encoding.UTF8,
                });
        }

        public void SendEmail(System.Net.Mail.MailMessage message)
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
            catch (ObjectDisposedException e)
            {
                this.Logger.ErrorException("Object Disposed when sending email", e);
            }
            catch (InvalidOperationException e)
            {
                this.Logger.ErrorException("Invalid Operation when sending email", e);
            }
            catch (ArgumentNullException e)
            {
                this.Logger.ErrorException("Null Argument when sending email", e);
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

        private ISmtpClientConfig Config
        {
            get { return this.currentConfig; }
        }

        private System.Net.Mail.SmtpClient MailClient
        {
            get { return this.currentMailClient ?? (this.currentMailClient = new System.Net.Mail.SmtpClient(this.Config.SmtpServer, this.Config.SmtpPort)); }
        }

        #endregion
    }
}