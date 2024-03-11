using Common.Database.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public interface IJobRepository
    {
        public Job? GetById(long id);
        public List<Job> GetByScheduler(long schedulerId);
        public List<Job> GetByOwner(long ownerId);
        public bool Create(Job job, out string message);
        public Job Update(Job job);
    }
}
