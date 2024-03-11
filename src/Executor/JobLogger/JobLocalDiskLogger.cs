using JobSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobExecutor.JobLogger
{
    /// <summary>
    /// Logger for log, write job to local file
    /// </summary>
    public class JobLocalDiskLogger : IJobLogger
    {
        enum LogLevel
        {
            Debug,
            Info, 
            Warning, 
            Error
        }

        private string logPath;
        private string className;

        public JobLocalDiskLogger(string logPath, string className)
        {
            this.logPath = logPath;
            this.className = className;
        }

        private void Log(LogLevel level, string message)
        {
            var fullMessage = $"[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}] [{className}] [{level}] {message}";
            using (StreamWriter outputFile = File.AppendText(logPath))
            {
                outputFile.WriteLine(fullMessage);
            }
        }
        public void LogDebug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void LogError(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void LogInformation(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void LogWarning(string message)
        {
            Log(LogLevel.Warning,message);
        }
    }
}
