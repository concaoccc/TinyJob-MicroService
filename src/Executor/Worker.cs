using Common.Database.PO;
using Common.Database.Repositories;
using Common.MessageQueue.Consumer;
using Common.MessageQueue.Message;
using Common.MessageQueue.Producer;
using Confluent.Kafka;
using JobExecutor.JobExecutor;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace JobExecutor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private readonly IJobRepository jobRepository;
    private readonly ISchedulerRepository schedulerRepository;
    private readonly IPackageRepository packageRepository;
    private readonly IExecutorRepository executorRepository;
    private readonly IMqConsumer consumer;
    private readonly IJobHost jobHost;
    private readonly Executor executor;



    public Worker( ILogger<Worker> logger, IServiceScopeFactory factory, IMqConsumer consumer, IJobHost jobHost)
    {
        this.jobRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IJobRepository>();
        this.schedulerRepository = factory.CreateScope().ServiceProvider.GetRequiredService<ISchedulerRepository>();
        this.packageRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IPackageRepository>();
        this.executorRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IExecutorRepository>();
        this.jobHost = jobHost;
        this.logger = logger;
        this.consumer = consumer;
        this.executor = GenerateExecutor();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            if (ThreadPool.PendingWorkItemCount > Environment.ProcessorCount)
            {
                Thread.Sleep(5 * 1000);
                continue;
            }

            var jobMessage = consumer.Consume();
            if (jobMessage == null) 
            {
                logger.LogWarning("Fetch a null object from MQ, will skip it.");
                continue;
            }

            try
            {
                ExecuteJob(jobMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }

    private void ExecuteJob(JobMessage jobMessage)
    {
        logger.LogInformation($"Fetch task {jobMessage}");
        var job = jobRepository.GetById(jobMessage.JobId);
        if (job == null)
        {
            logger.LogError($"Can't find {jobMessage.JobId} from job table, will pass this job.");
            return;
        }

        if (job.Status != JobStatus.Schedulered && job.Status != JobStatus.NotStarted)
        {
            logger.LogError($"job {job.Name}'s status is {job.Status}, no need to execute it.");
            return;
        }

        job.Status = JobStatus.WaitForExectution;
        job.ExecutorId = executor.Id;
        job = jobRepository.Update(job);
        consumer.Commit();
        var jobInfo = GetJobExecutionInfo(job);
        if (jobInfo == null)
        {
            logger.LogError("Can't generate job info, will pass this job.");
        }
        else
        {
            job.Status = JobStatus.WaitForExectution;
            jobRepository.Update(job);
            ThreadPool.QueueUserWorkItem(state => jobHost.Execute(jobInfo));
        }
    }

    private JobExecutionInfo? GetJobExecutionInfo(Job job)
    {
        var scheduler = job.Scheduler;
        if (scheduler == null)
        {
            scheduler = schedulerRepository.GetById(job.SchedulerId);
            if (scheduler == null)
            {
                logger.LogError($"Can't find scheduler {job.SchedulerId} for job {job.Name}, no need to execute it.");
                return null;
            }
        }

        var package = scheduler.Package;
        if (package == null)
        {
            package = packageRepository.GetById(scheduler.PackageId);
            if (package == null)
            {
                logger.LogError($"Can't find package {scheduler.PackageId} for job {scheduler.Name}, no need to execute it.");
                return null;
            }
        }
        var jobInfo = new JobExecutionInfo
        {
            JobId = job.Id,
            JobName = job.Name,
            PackageName = package.Name,
            PackageVersion = package.Version,
            AssemblyName = scheduler.AssemblyName,
            Namespace = scheduler.Namespace,
            ClassName = scheduler.ClassName,
            ScheduleredExecutionTime = job.ScheduledExecutionTime.Value
        };

        return jobInfo;
    }

    private Executor GenerateExecutor()
    {
        var executor = new Executor
        {
            Name = Guid.NewGuid().ToString(),
            LastHeartbeat = DateTime.Now,
            Status = ExecutorStatus.online
        };
        return executorRepository.Create(executor);
    }
}
