using ServerUpload.BLL.Enums;
using Version = ServerUpload.BLL.BusinessModels.Version;

namespace ServerUpload.BLL.Interfaces
{   
    public interface IVersionsService
    {
        public Version CreateVersion(byte [] fileBytes, string name, Categories category, long size, string path, string strHash, string fileName);
        public string DownloadVersion(int number, string name, Categories category);

        public string GetHash(byte[] fileBytes);
        public string GetPath(Categories category, string materialName, string hashString, string fileName);
    }
}
