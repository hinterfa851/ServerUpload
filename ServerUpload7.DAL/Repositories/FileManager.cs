using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.Interfaces;

namespace ServerUpload7.DAL.Repositories
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
