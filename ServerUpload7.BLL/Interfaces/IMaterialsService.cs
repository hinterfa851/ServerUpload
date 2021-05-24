using System.Collections.Generic;
using AutoMapper;
using ServerUpload7.BLL.BusinessModels;

namespace ServerUpload7.BLL.Interfaces
{
    public interface IMaterialsService
    {
        public Material CreateMaterial(byte[] fileBytes, int category, string fileName, long size, string path, string hashString);
        public string GetPath(int category, string fileName, int number, string hashString);
        public string DownloadActualVersion(string name, int category);
        public Material GetMaterialInfo(string name, int category);
        public int ChangeCategory(string name, int oldCategory, int newCategory);
        public IEnumerable<Material> FilterMat(int category);

        public string GetHash(byte[] fileBytes);
    }
}
