namespace ServerUpload.DAL.Interfaces
{
    public interface IFileManager
    {
        public void CreateDirectory(string path);

        public void SaveFile(string path, byte[] fileBytes);

        public string GetHash(byte[] fileBytes);
    }
    
}
