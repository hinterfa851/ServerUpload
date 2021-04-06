using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.Interfaces;
using ServerUpload7.DAL.Entities;
using ServerUpload7.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Version = ServerUpload7.DAL.Entities.Version;


namespace ServerUpload7.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        // **** USELESS???? *****
        private ApplicationContext db;
        private MaterialsRepository materialRepository;
        private VersionsRepository versionRepository;

        public EFUnitOfWork(DbContextOptions connectionString)
        {
            db = new ApplicationContext(connectionString);
        }
        public IRepository<Material> Materials
        {
            get
            {
                if (materialRepository == null)
                    materialRepository = new MaterialsRepository(db);
                return materialRepository;
            }
        }

        public IRepository<Version> Versions
       // public VersionsRepository Versions
        {
            get
            {
                if (versionRepository == null)
                    versionRepository = new VersionsRepository(db);
                return versionRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
