using Matslump.Models;
using Matslump.Services;
using System;
using System.IO;
using System.Reflection;

namespace Matslump.Tools
{
    public class LogWriter
    { 
        public LogWriter(string logMessage)
        {
            //LogWrite(logMessage);
           Email.SendEmail("gorlingy@hotmail.com", "Serverfel", "Serverfel", logMessage);
        }
    }
}