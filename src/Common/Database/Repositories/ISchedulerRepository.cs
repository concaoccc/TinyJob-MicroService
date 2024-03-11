using Common.Database.PO;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public interface ISchedulerRepository
    {
        public Scheduler? GetById(long id);
        public List<Scheduler> GetByPackage(long packageId);
        public List<Scheduler> GetByNextExecutionTimeBefore(DateTime dateTime);
        public List<Scheduler> GeyByOwner(long ownerId);
        public bool Create(Scheduler scheduler, out string message);
        public bool Update(Scheduler scheduler, out string message);
        public void DeleteById(long id);
        public bool ExecutionPlanIsValid(Scheduler scheduler);
        public DateTime GetNextExecutionTime(Scheduler scheduler);
    }
}
