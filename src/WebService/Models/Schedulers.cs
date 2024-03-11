using Common.Database.PO;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
    public class Schedulers
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string PackageName { get; set; }
        public string PackageVersion { get; set; }
        public string AssemblyName { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string ExecutionPlan { get; set; }
        public string? ExecutionParams { get; set; }
        public DateTime EndTime { get; set; }
    }
}
