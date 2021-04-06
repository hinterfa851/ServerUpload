using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ServerUpload7.DAL.Entities;

namespace ServerUpload7.DAL.Services
{
    public interface IMaterialsService
    {
        public Material CreateMaterial(Stream uploadedFile, string Category, string FileName, string WebRootPath, long Size);
        public string GetPath(string category, string FileName, int number);
        public string DownloadActualVersion(string Name, string Category);
        public Material GetMaterialInfo(string Name, string Category);
        public int ChangeCategory(string Name, string OldCategory, string NewCategory);
        public IEnumerable<Material> FilterMat(string Category);
    }
}