using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.Entities;
using Version = ServerUpload7.DAL.Entities.Version;

namespace ServerUpload7.DAL.Services
{
    public interface IVersionsService
    {
        public Version CreateVersion(string Name, string Category, string WebRootPath, long Size);
        public string DownloadVers(int number, string name, string category);

        public string GetPath(string category, string FileName);
    }
}
