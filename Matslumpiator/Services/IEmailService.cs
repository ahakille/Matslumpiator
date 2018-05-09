using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matslumpiator.Services
{
    public interface IEmailService
    {
        void SendEmail(string epost, string name, string subject, string body);
    }
}
