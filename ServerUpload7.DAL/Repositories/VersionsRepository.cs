using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.EF;
using ServerUpload7.DAL.Entities;
using ServerUpload7.DAL.Interfaces;
using Version = ServerUpload7.DAL.Entities.Version;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ServerUpload7.DAL.Repositories
{
       public class VersionsRepository : IRepository<Version>
    {
        private ApplicationContext db;

        public VersionsRepository(ApplicationContext context)
        {
            this.db = context;
        }
        
        public IEnumerable<Version> GetAll(Func<Version, Boolean> predicate)
        {
            var ff = db.Versions.Include(o => o.Material);
            return ff.Where(predicate);
        }

        public Version Get(int id)
        {
            return db.Versions.Find(id);
        }

        public Version Create(Version version)
        {
            db.Versions.Add(version);
            return version;
        }
        
        public Version Find(Func<Version, Boolean> predicate)
        {
            
            return db.Versions.FirstOrDefault(predicate);
        }
        public void Delete(int id)
        {
            Version order = db.Versions.Find(id);
            if (order != null)
                db.Versions.Remove(order);
        }
        
        public Version Update(Version version)
        {
            return version;
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
