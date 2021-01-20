using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace JAF.integration.service
{
    public class LogManager
    {
        private string m_exePath = string.Empty;
        public LogManager()
        {
        }
        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                bool folderExists = Directory.Exists(m_exePath + "\\Logs");
                if (!folderExists)
                    Directory.CreateDirectory(m_exePath + "\\Logs");

                using StreamWriter w = File.AppendText(m_exePath + "\\Logs\\" + "log" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                Log(logMessage, w);
            }
            catch
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), logMessage);
            }
            catch
            {
            }
        }

        public string GetLastFileLog()
        {
            return (m_exePath + "\\Logs\\" + "log" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
        }
    }
}
