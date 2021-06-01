using System;
using ServerUpload.DAL.Entities;
using Version = ServerUpload.DAL.Entities.Version;

namespace ServerUpload.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
    
        public IRepository<Material> Materials { get; }
        public IRepository<Version> Versions { get; }

        public IFileManager FileManager { get; }
    }
}
