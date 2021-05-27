using System.Collections.Generic;
using ServerUpload7.BLL.BusinessModels;

namespace ServerUpload7.BLL.Interfaces
{
    public interface IMaterialsService
    {
        public Material CreateMaterial(byte[] fileBytes, byte category, string fileName, long size, string path, string hashString);
        public string GetPath(byte category, string fileName, int number, string hashString);
        public string DownloadActualVersion(string name, byte category);
        public Material GetMaterialInfo(string name, byte category);
        public Material ChangeCategory(string name, byte oldCategory, byte newCategory);
        public IEnumerable<Material> FilterMat(byte category);
        public string GetHash(byte[] fileBytes);
    }
}
