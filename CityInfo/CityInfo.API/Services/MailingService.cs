using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class MailingService : IMailingService
    {
        private readonly string  _mailTo = Startup.Configuration["mailSettings:ToMail"];
        private readonly string _mailFrom = Startup.Configuration["mailSettings:FromMail"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"mail from {_mailFrom} to {_mailTo} via custom Mail_Service");
            Debug.WriteLine($"The subject is {subject}");
            Debug.WriteLine($"The message is {message}");
        }
    }
}
