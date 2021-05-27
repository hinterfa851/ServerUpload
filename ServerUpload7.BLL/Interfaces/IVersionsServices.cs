using Version = ServerUpload7.BLL.BusinessModels.Version;

namespace ServerUpload7.BLL.Interfaces
{   
    public interface IVersionsService
    {
        public Version CreateVersion(byte [] fileBytes, string name, byte category, long size, string path, string strHash, string fileName);
        public string DownloadVersion(int number, string name, byte category);

        public string GetHash(byte[] fileBytes);
        public string GetPath(byte category, string materialName, string hashString, string fileName);
    }
}
