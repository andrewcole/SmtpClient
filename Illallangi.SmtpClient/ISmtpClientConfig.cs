namespace Illallangi
{
    public interface ISmtpClientConfig
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
    }
}