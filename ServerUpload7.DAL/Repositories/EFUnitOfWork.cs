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
        
        private ApplicationContext db;
        private MaterialsRepository materialRepository;
        private VersionsRepository versionRepository;
        private FileManager fileManager;

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
        {
            get
            {
                if (versionRepository == null)
                    versionRepository = new VersionsRepository(db);
                return versionRepository;
            }
        }

        public FileManager FileManager
        {
            get
            {
                if (fileManager == null)
                {
                    fileManager = new FileManager();
                }

                return fileManager;
            }
        }

        public List<Category> GetCategories()
        {
            //List<int> result = new List<int>();
            //foreach (var name in db.Category)
            //{
            //    result.Add(name.Id);
            //} 
            //return result.AsEnumerable();
            List<Category> result = new List<Category>();
            foreach (var category in db.Category)
            {
                result.Add(category);
            }
            return result;
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
