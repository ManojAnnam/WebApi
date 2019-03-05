using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface IMailingService
    {
        void send(string subject, string message);
    }
}
