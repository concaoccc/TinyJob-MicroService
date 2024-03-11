using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Database.PO
{
    public enum JobStatus
    {
        NotStarted = 1,
        Schedulered = 2,
        WaitForExectution = 3,
        Executing = 4,
        Succeed = 5,
        Failed = 6,
        Paused = 7,
        Stopped = 8,
        ReScheduled = 9
    }
    public class Job
    {
        [Key]
        public long Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
        [ForeignKey("Scheduler")]
        public long SchedulerId { get; set; }
        public virtual Scheduler Scheduler { get; set; }
        public long? ExecutorId { get; set; }
        public JobStatus Status { get; set; }
        public DateTime? ScheduledExecutionTime { get; set; }
        public DateTime? ActualExecutionTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}