using AutoMapper;
using Version = ServerUpload7.BLL.BusinessModels.Version;

namespace ServerUpload7.BLL.Interfaces
{   
    public interface IVersionsService
    {
        public Version CreateVersion(byte [] fileBytes, string name, int category, long size, IMapper mapper, string path, string strHash, string fileName);
        public string DownloadVersion(int number, string name, int category, IMapper mapper);

        public string GetPath(int category, string materialName, IMapper mapper, string hashString, string fileName);
    }
}
