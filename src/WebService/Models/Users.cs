using Common.Database.PO;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
    public class Users
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
