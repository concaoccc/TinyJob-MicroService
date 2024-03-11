using JobExecutor.JobLogger;
using JobSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JObExecutor.JobExecutor
{
    public class JobContext : IJobContext
    {
        private long jobId;
        private string className;
        private IJobLogger logger;

        public JobContext(long jobId, string className) 
        {
            this.jobId = jobId;
            this.className = className;
            logger = JobLoggerFactory.Create(jobId, className);
        }

        public void LogDebug(string message)
        {
            logger.LogDebug(message);
        }

        public void LogError(string message)
        {
            logger.LogError(message);
        }

        public void LogInformation(string message)
        {
            logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            logger.LogWarning(message);
        }
    }
}
