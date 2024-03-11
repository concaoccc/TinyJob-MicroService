using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Database.PO
{
    [Index(nameof(OwnerId), nameof(Name), nameof(Version), IsUnique = true)]
    public class Package
    {
        [Key]
        public long Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
        public string Version { get; set; }
        public string StorageAccount { get; set; }
        public string RelativePath { get; set; }
        [ForeignKey("User")]
        public long OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}