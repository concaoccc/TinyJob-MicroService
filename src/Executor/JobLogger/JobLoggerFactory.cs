using JobSDK;

namespace JobExecutor.JobLogger
{
    public static class JobLoggerFactory
    {
        public static IJobLogger Create(long jobId, string className)
        {
            var logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "TaskManagement", "Job");
            Directory.CreateDirectory( logDirectory );
            var logPath = Path.Combine(logDirectory, $"{jobId}.log");
            return new JobLocalDiskLogger(logPath, className);
        }
    }
}
