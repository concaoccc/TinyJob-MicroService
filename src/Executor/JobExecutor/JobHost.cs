using Common.Database.PO;
using Common.Database.Repositories;
using JObExecutor.JobExecutor;
using JobSDK;
using System.Reflection;

namespace JobExecutor.JobExecutor
{
    public class JobHost : IJobHost
    {
        private ILogger logger;
        private string baseWorkDirectory;
        private IJobRepository jobRepository;
        private const string ExecutionFunctionName = "Execute";

        public JobHost(ILogger<JobHost> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.jobRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IJobRepository>();
            this.baseWorkDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "TaskManagement");
            Directory.CreateDirectory(baseWorkDirectory);
        }

        public void Execute(JobExecutionInfo jobInfo)
        {
            var datetimeDiffInMilliseconds = (int)(jobInfo.ScheduleredExecutionTime - DateTime.Now).TotalMilliseconds;
            if (datetimeDiffInMilliseconds > 0) 
            {
                logger.LogInformation($"Will exectue {jobInfo.JobName} after {datetimeDiffInMilliseconds/1000} seconds.");
                Thread.Sleep(datetimeDiffInMilliseconds);
            }

            logger.LogInformation($"Begin to execute job {jobInfo.JobName}.");
            var job = jobRepository.GetById(jobInfo.JobId);
            job.Status = JobStatus.Executing;
            job.ActualExecutionTime = DateTime.UtcNow;
            jobRepository.Update(job);
            var packagePath = CheckPackage(jobInfo);
            if (string.IsNullOrEmpty(packagePath) )
            {
                throw new ArgumentException($"Can't find {jobInfo.PackageName}-{jobInfo.PackageVersion} in the host.");
            }

            var assmeblyPath = Path.Combine(packagePath, $"{jobInfo.AssemblyName}.dll");
            var assembly = Assembly.LoadFile(assmeblyPath);
            var type = assembly.GetType($"{jobInfo.Namespace}.{jobInfo.ClassName}");
            if (type == null)
            {
                throw new ArgumentException($"Can't find {jobInfo.Namespace}.{jobInfo.ClassName} from {assmeblyPath}.");
            }

            var methodInfo = type.GetMethod(typeof(JobSDK.Job).GetMethods()[0].Name);
            if (methodInfo == null)
            {
                throw new ArgumentException($"Can't find Execute function in {type.FullName}");
            }

            var jobOject = Activator.CreateInstance(type, null);
            if (jobOject == null)
            {
                throw new Exception($"Can't create instance for {type.FullName}");
            }

            var paramameterType = jobOject.GetType();
            if (!paramameterType.GetMembers().Select(m => m.Name).Contains(ExecutionFunctionName))
            {
                throw new Exception($"Can't find {ExecutionFunctionName} in the {type.FullName}");
            }

            var jobContext = PrepareJobContext(jobInfo);
            object[] parametersArray = new object[] { jobContext };
            try
            {
                paramameterType.InvokeMember("Execute",
                    BindingFlags.InvokeMethod,
                    null,
                    jobOject,
                    parametersArray);
                    job.Status = JobStatus.Succeed;
            }
            catch (Exception ex)
            {
                jobContext.LogError(ex.Message);
                job.Status = JobStatus.Failed;
                
            }
            finally
            {
                job.FinishTime = DateTime.UtcNow;
                jobRepository.Update(job);
            }
        }

        /// <summary>
        /// Download package to workdirectory
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string CheckPackage(JobExecutionInfo jobInfo)
        {
            var packagePath = Path.Combine(baseWorkDirectory, "Packages", jobInfo.PackageName, jobInfo.PackageVersion);
            logger.LogInformation($"Use {packagePath} as package base folder.");
            var assmeblyPath = Path.Combine(packagePath, $"{jobInfo.AssemblyName}.dll");
            if (File.Exists(assmeblyPath))
            {
                logger.LogInformation($"Find {assmeblyPath}.");
                return packagePath;
            }
            else
            {
                logger.LogError($"Can't find {assmeblyPath}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Download the target package-version from Storage Account
        /// </summary>
        /// <param name="destintionPath"></param>
        private void DownloadPackage(string destintionPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Init Job context for this job
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private IJobContext PrepareJobContext(JobExecutionInfo jobInfo)
        {
            return new JobContext(jobInfo.JobId, jobInfo.ClassName);
        }
    }
}
