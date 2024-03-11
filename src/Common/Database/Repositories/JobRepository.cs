using Common.Database.PO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public class JobRepository : IJobRepository
    {
        private AppDbContext dbContext { get; }
        private ILogger logger { get; }

        public JobRepository(AppDbContext dbContext, ILogger<JobRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public bool Create(Job job, out string message)
        {
            try
            {
                dbContext.Jobs.Add(job);
                dbContext.SaveChanges();
                message = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public Job? GetById(long id)
        {
            return dbContext.Jobs.Find(id);
        }

        public List<Job> GetByOwner(long ownerId)
        {
            return dbContext.Jobs.Where(j => j.Scheduler.Package.OwnerId == ownerId).ToList();
        }

        public List<Job> GetByScheduler(long schedulerId)
        {
            return dbContext.Jobs.Where(j => j.SchedulerId == schedulerId).ToList();
        }

        public Job Update(Job job)
        {
            dbContext.Jobs.Update(job);
            dbContext.SaveChanges();
            return job;
        }
    }
}
