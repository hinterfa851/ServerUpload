using System;
using System.Collections.Generic;
using System.Linq;
using ServerUpload7.DAL.EF;
using ServerUpload7.DAL.Interfaces;
using Version = ServerUpload7.DAL.Entities.Version;
using Microsoft.EntityFrameworkCore;

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
            return db.Versions.Include(o => o.Material).Where(predicate);
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
            var dbEntity = db.Versions.FirstOrDefault(m => m.Id == version.Id);
            dbEntity.Name = version.Name;
            dbEntity.MaterialId = version.MaterialId;
            dbEntity.FileSize = version.FileSize;
            dbEntity.StrHash = version.StrHash;
            dbEntity.Material = version.Material;
            db.SaveChanges();
            return dbEntity;
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
