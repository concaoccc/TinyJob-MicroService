using Common.Database.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public class ExecutorRepository : IExecutorRepository
    {
        private AppDbContext dbContext { get; }

        public ExecutorRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Executor Create(Executor executor)
        {
            dbContext.Executors.Add(executor);
            dbContext.SaveChanges();
            return executor;
        }

        public Executor? GetById(long id)
        {
            return dbContext.Executors.Find(id);
        }

        public List<Executor> GetByLastHeartBeatBefore(DateTime dateTime)
        {
            return dbContext.Executors.Where(e => e.LastHeartbeat < dateTime).ToList();
        }

        public Executor Update(Executor executor)
        {
            dbContext.Executors.Update(executor);
            dbContext.SaveChanges();
            return executor;
        }

        public Executor? GetByName(string name)
        {
            return dbContext.Executors.FirstOrDefault(e => e.Name == name);
        }
    }
}
