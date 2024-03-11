using Common.Database.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public interface IExecutorRepository
    {
        public Executor? GetById(long id);
        public Executor? GetByName(string name);
        public List<Executor> GetByLastHeartBeatBefore(DateTime dateTime);
        public Executor Create(Executor Executor);
        public Executor Update(Executor Executor);
    }
}
