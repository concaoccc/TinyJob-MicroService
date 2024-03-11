using Common.Database.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public interface IUserRepository
    {
        public User? GetById(long id);
        public User? GetByUserName(string userName);
        public bool Create(User user, out string message);
        public bool Update(User user, out string message);
    }
}
