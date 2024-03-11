namespace Scheduler
{
    using Common.Database.PO;
    using Common.Database.Repositories;
    using Common.MessageQueue.Message;
    using Common.MessageQueue.Producer;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ISchedulerRepository schedulerRepository;
        private readonly IMqProducer mqProduer;
        private readonly IJobRepository jobRepository;
        private readonly TimeSpan Interval = TimeSpan.FromMinutes(2);

        public Worker(ILogger<Worker> logger, IServiceScopeFactory factory, IMqProducer mqProduer)
        {
            this.mqProduer = mqProduer;
            this.logger = logger;
            this.schedulerRepository = factory.CreateScope().ServiceProvider.GetRequiredService<ISchedulerRepository>();
            this.jobRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IJobRepository>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var schedulers = schedulerRepository.GetByNextExecutionTimeBefore(DateTime.UtcNow + Interval);

                var jobs = ProcessSchedulers(schedulers);                               
                SendToMessageQueue(jobs);

                await Task.Delay(900, stoppingToken);
            }
        }

        private async void SendToMessageQueue(List<Job> jobs)
        {
            var jobMessages = new List<JobMessage>();

            foreach (var job in jobs)
            {
                jobMessages.Add(
                    new JobMessage { JobId = job.Id}
                    );
            }
            await mqProduer.ProduceAsync(jobMessages);
        }

        private List<Job> ProcessSchedulers(List<Scheduler> schedulers)
        {
            var jobs = new List<Job>();
            foreach (var scheduler in schedulers)
            {
                if (schedulerRepository.ExecutionPlanIsValid(scheduler))
                {
                    var nextExecutionTime = schedulerRepository.GetNextExecutionTime(scheduler);

                    scheduler.NextExecutionTime = nextExecutionTime;
                    scheduler.UpdateTime = DateTime.UtcNow;

                    if (schedulerRepository.Update(scheduler, out string message))
                    {
                        logger.LogInformation(message);
                        var job = new Job
                        {
                            Status = JobStatus.NotStarted,
                            Name = scheduler.Name,
                            SchedulerId = scheduler.Id,
                            ScheduledExecutionTime = nextExecutionTime,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        if (jobRepository.Create(job, out message))
                        {
                            jobs.Add(job);
                            logger.LogInformation(message);
                        }
                        else
                        {
                            logger.LogWarning(message);
                        }
                    }
                }
                else
                {
                    string message = $"Execution plan {scheduler.ExecutionPlan} of {scheduler.Name} is invalid";
                    logger.LogWarning(message);
                }
            }
            return jobs;
        }
    }
}