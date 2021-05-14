using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.Entities;
using Version = ServerUpload7.DAL.Entities.Version;
using ServerUpload7.DAL.Repositories;

namespace ServerUpload7.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
    
        public IRepository<Material> Materials { get; }
        public IRepository<Version> Versions { get; }

        public IEnumerable<int> GetCategories();
    }
}
