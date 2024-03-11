using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Common.Database.PO
{
    [Index(nameof(Name), IsUnique = true)]
    public class User
    {
        [Key]
        public long Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
        public string Pwd { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? Email { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}