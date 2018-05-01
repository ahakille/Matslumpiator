using Matslumpiator.Models;
using Matslumpiator.Services;
using System;
using System.IO;
using System.Reflection;

namespace Matslumpiator.Tools
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        public LogWriter(string logMessage)
        {
           Email.SendEmail("gorlingy@hotmail.com", "Serverfel", "Serverfel", logMessage);
        }
      

        
    }
}