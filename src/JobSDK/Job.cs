using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSDK
{
    public abstract class Job
    {
        abstract public void Execute(IJobContext context);
    }
}
