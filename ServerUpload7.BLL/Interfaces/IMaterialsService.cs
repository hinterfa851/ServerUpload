using System.Collections.Generic;
using AutoMapper;
using ServerUpload7.BLL.BusinessModels;

namespace ServerUpload7.BLL.Interfaces
{
    public interface IMaterialsService : ICommon
    {
        public Material CreateMaterial(byte[] fileBytes, string category, string fileName, long size, IMapper mapper, string path, string hashString);
        public string GetPath(string category, string fileName, int number, IMapper mapper, string hashString);
        public string DownloadActualVersion(string name, string category, IMapper mapper);
        public Material GetMaterialInfo(string name, string category, IMapper mapper);
        public int ChangeCategory(string name, string oldCategory, string newCategory, IMapper mapper);
        public IEnumerable<Material> FilterMat(string category, IMapper mapper);
    }
}
