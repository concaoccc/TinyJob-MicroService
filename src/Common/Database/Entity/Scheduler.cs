using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Database.PO
{
    public enum SchedulerType
    {
        Once = 1,
        Cron = 2,
        Interval = 3
    }

    [Index(nameof(NextExecutionTime))]
    [Index(nameof(PackageId), nameof(Name), IsUnique = true)]
    public class Scheduler
    {
        [Key]
        public long Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
        public SchedulerType Type { get; set; }
        [ForeignKey("package")]
        public long PackageId { get; set; }
        public virtual Package Package { get; set; }
        public string AssemblyName { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string ExecutionPlan { get; set; }
        public string? ExecutionParams { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime? NextExecutionTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}