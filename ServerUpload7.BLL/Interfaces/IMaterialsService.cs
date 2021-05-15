using System.Collections.Generic;
using AutoMapper;
using ServerUpload7.BLL.BusinessModels;

namespace ServerUpload7.BLL.Interfaces
{
    public interface IMaterialsService
    {
        public Material CreateMaterial(byte[] fileBytes, int category, string fileName, long size, IMapper mapper, string path, string hashString);
        public string GetPath(int category, string fileName, int number, IMapper mapper, string hashString);
        public string DownloadActualVersion(string name, int category, IMapper mapper);
        public Material GetMaterialInfo(string name, int category, IMapper mapper);
        public int ChangeCategory(string name, int oldCategory, int newCategory, IMapper mapper);
        public IEnumerable<Material> FilterMat(int category, IMapper mapper);

        public string GetHash(byte[] fileBytes);
    }
}
