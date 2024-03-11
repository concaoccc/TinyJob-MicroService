namespace Common.Database.Repositories
{
    using Common.Database.PO;
    using System.Text.RegularExpressions;

    public class SchedulerRepository : ISchedulerRepository
    {
        private AppDbContext dbContext { get; }

        public SchedulerRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool Create(Scheduler scheduler, out string messgae)
        {
            try
            {
                dbContext.Schedulers.Add(scheduler);
                dbContext.SaveChanges();
                messgae = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                messgae = ex.Message;
                return false;
            }
        }

        public void DeleteById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Scheduler> GetByNextExecutionTimeBefore(DateTime dateTime)
        {
            return dbContext.Schedulers.Where(s => s.NextExecutionTime < dateTime).ToList();
        }

        public Scheduler? GetById(long id)
        {
            return dbContext.Schedulers.Find(id);
        }

        public List<Scheduler> GetByPackage(long packageId)
        {
            return dbContext.Schedulers.Where(s => s.PackageId == packageId).ToList();
        }

        public List<Scheduler> GeyByOwner(long ownerId)
        {
            return dbContext.Schedulers.Where(s => s.Package.OwnerId == ownerId).ToList();
        }

        public bool Update(Scheduler scheduler, out string message)
        {
            try
            {
                dbContext.Schedulers.Update(scheduler);
                dbContext.SaveChanges();
                message = string.Empty;
                return true;
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool ExecutionPlanIsValid(Scheduler scheduler)
        {
            //only support format in Seconds Minutes Hours Days Months e.g. [5 * * *] means every five minutes, * means 0
            string pattern = "^([1-5]?[0-9]|0) ([1-5]?[0-9]|0) ([1-5]?[0-9]|0) ([1-5]?[0-9]|0) ([1-5]?[0-9]|0)$";
            Match match = Regex.Match(scheduler.ExecutionPlan, pattern);
            return match.Success;
        }

        public DateTime GetNextExecutionTime(Scheduler scheduler)
        {
            // if type is once, and execution time is empty, next execution time is inifinite
            if (scheduler.Type == SchedulerType.Once && scheduler.NextExecutionTime is null)
            {
                return DateTime.MaxValue;
            }

            // if type is cron, get next execution time by parse execution plan and create time
            if (scheduler.Type == SchedulerType.Cron)
            {

                if (scheduler.NextExecutionTime is not null)
                {
                    return GetNextExecutionTimeForCron((DateTime)scheduler.NextExecutionTime, scheduler.ExecutionPlan, scheduler.EndTime);
                }
                else {
                    return GetNextExecutionTimeForCron(scheduler.CreateTime, scheduler.ExecutionPlan, scheduler.EndTime);
                }    
            }

            // scheduler.Interval not supported yet
            throw new ArgumentException($"Unsupported scheduler type", nameof(scheduler.Type));
        }

        private DateTime GetNextExecutionTimeForCron(DateTime executionTime, string cron, DateTime endTime)
        {
            var numbers = cron.Split();
            executionTime = executionTime.AddSeconds(int.Parse(numbers[0])).AddMinutes(int.Parse(numbers[1])).AddHours(int.Parse(numbers[2])).AddDays(int.Parse(numbers[3])).AddMonths(int.Parse(numbers[4]));
            return GetVerifiedExecutionTime(executionTime, endTime);
        }

        private DateTime GetVerifiedExecutionTime(DateTime executionTime, DateTime endTime)
        { 
            return executionTime > endTime ? DateTime.MaxValue : executionTime;
        }
    }
}
