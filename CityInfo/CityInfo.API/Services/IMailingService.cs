using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface IMailingService
    {
        void Send(string subject, string message);
    }
}
