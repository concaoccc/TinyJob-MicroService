using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobExecutor.JobExecutor
{
    public class JobExecutionInfo
    {
        public long JobId { get; set; }
        public string JobName { get; set; }
        public string PackageName { get; set; }
        public string PackageVersion { get; set; }
        public string? storageAccountName { get; set; }
        public string? RelativePath { get; set; }
        public string AssemblyName { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public DateTime ScheduleredExecutionTime { get; set; }
    }
}
