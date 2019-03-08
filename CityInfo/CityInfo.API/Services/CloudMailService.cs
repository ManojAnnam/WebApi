using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailingService
    {
        private string _mailTo = "annam.manoj1996@gmail.com";
        private string _mailFrom = "svads99@gmail.com";
        public void Send(string subject, string message)
        {
            Debug.WriteLine($"mail from {_mailFrom} to {_mailTo} via custom Cloud_Mail_Service");
            Debug.WriteLine($"The subject is {subject}");
            Debug.WriteLine($"The message is {message}");
        }
    }
}
