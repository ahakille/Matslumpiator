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
            //LogWrite(logMessage);
           Email.SendEmail("gorlingy@hotmail.com", "Serverfel", "Serverfel", logMessage);
        }
        public void LogWrite(string logMessage)
        {//Assembly.GetExecutingAssembly().Location
            m_exePath = Path.GetDirectoryName(@"c:\inetpub\wwwroot\matslumpiator\logs\");
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}