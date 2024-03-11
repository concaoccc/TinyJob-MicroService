using Common.Database.PO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database.Repositories
{
    public interface IPackageRepository
    {
        public Package? GetById(long id);
        public List<Package> GetByName(string name, long ownerId);
        public Package? GetByNameAndVersion(string name, string version, long ownerId);
        public List<Package> GetByOwner(long ownerId);
        public bool Create(Package package, out string message);
        public bool Update(Package package, out string message);
    }
}
