using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobExecutor.JobExecutor
{
    public interface IJobHost
    {
        void Execute(JobExecutionInfo jobInfo);
    }
}
