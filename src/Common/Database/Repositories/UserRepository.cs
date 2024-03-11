using Common.Database.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext dbContext { get; }

        public UserRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool Create(User user, out string message)
        {
            try
            {
                dbContext.User.Add(user);
                dbContext.SaveChanges();
                message = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public User? GetById(long id)
        {
            return dbContext.User.Find(id);
        }

        public User? GetByUserName(string userName)
        {
            return dbContext.User.Where(u => u.Name == userName).FirstOrDefault();
        }

        public bool Update(User user, out string message)
        {
            try
            {
                dbContext.User.Update(user);
                dbContext.SaveChanges();
                message = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}
