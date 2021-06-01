using System;
using System.IO;
using System.Security.Cryptography;
using ServerUpload.DAL.Interfaces;

namespace ServerUpload.DAL.Repositories
{
    public class FileManager : IFileManager
    {
        public void SaveFile(string path, byte[] fileBytes)
        {
            File.WriteAllBytes(path, fileBytes);
        }

        public string GetHash(byte[] fileBytes)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(fileBytes);
            md5.Dispose();
            return Convert.ToBase64String(hash);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
