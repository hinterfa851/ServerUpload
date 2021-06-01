using System.Collections.Generic;
using ServerUpload.BLL.BusinessModels;
using ServerUpload.BLL.Enums;

namespace ServerUpload.BLL.Interfaces
{
    public interface IMaterialsService
    {
        public Material CreateMaterial(byte[] fileBytes, Categories category, string fileName, long size, string path, string hashString);
        public string GetPath(Categories category, string fileName, int number, string hashString);
        public string DownloadActualVersion(string name, Categories category);
        public Material GetMaterialInfo(string name, Categories category);
        public Material ChangeCategory(string name, Categories oldCategory, Categories newCategory);
        public IEnumerable<Material> FilterMat(Categories category);
        public string GetHash(byte[] fileBytes);
    }
}
