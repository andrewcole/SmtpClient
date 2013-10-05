namespace Illallangi
{
    public sealed class NullSmtpClientConfig : ISmtpClientConfig
    {
        public string SmtpServer
        {
            get { return "localhost"; }
        }
        
        public int SmtpPort
        {
            get { return 25; }
        }
    }
}