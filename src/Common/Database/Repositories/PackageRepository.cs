using Common.Database.PO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private AppDbContext dbContext { get; }
        private readonly ILogger<PackageRepository> logger;

        public PackageRepository(AppDbContext dbContext, ILogger<PackageRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public Package? GetById(long id)
        {
            return dbContext.Packages.Find(id);
        }

        public List<Package> GetByName(string name, long ownerId)
        {
            return dbContext.Packages.Where(p => p.Name == name && p.OwnerId == ownerId).ToList();
        }

        public Package? GetByNameAndVersion(string name, string version, long ownerId)
        {
            return dbContext.Packages.FirstOrDefault(p => p.Name == name && p.Version == version && p.OwnerId == ownerId);
        }

        public List<Package> GetByOwner(long ownerId)
        {
            return dbContext.Packages.Where(p => p.OwnerId == ownerId).ToList();
        }

        public bool Create(Package package, out string message)
        {
            try
            {
                dbContext.Packages.Add(package);
                dbContext.SaveChanges();
                message = string.Empty;
                return true;
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool Update(Package package, out string message)
        {
            try
            {
                dbContext.Packages.Update(package);
                dbContext.SaveChanges();
                message = string.Empty;
                return true;
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}
