using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Common.Database.PO
{
    public enum ExecutorStatus
    {
        online = 1,
        Offline = 2
    }

    [Index(nameof(LastHeartbeat))]
    public class Executor
    {
        [Key]
        public long Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime LastHeartbeat { get; set; }
        public ExecutorStatus Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}