﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class MailingService
    {
        private string _mailTo = "annam.manoj1996@gmail.com";
        private string _mailFrom = "svads99@gmail.com";

        public void send (string subject,string message)
        {
            Debug.WriteLine($"mail from {_mailFrom} to {_mailTo} via custom Mail_Service");
            Debug.WriteLine($"The subject is {subject}");
            Debug.WriteLine($"The message is {message}"); 
        }
    }
}