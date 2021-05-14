using AutoMapper;
using Version = ServerUpload7.BLL.BusinessModels.Version;

namespace ServerUpload7.BLL.Interfaces
{   
    public interface IVersionsService : ICommon
    {
        public Version CreateVersion(byte [] fileBytes, string name, string category, long size, IMapper mapper, string path, string strHash, string fileName);
        public string DownloadVersion(int number, string name, string category, IMapper mapper);

        public string GetPath(string category, string materialName, IMapper mapper, string hashString, string fileName);
    }
}
