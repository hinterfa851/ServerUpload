using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerUpload7.DAL.Interfaces
{
    public interface IFileManager
    {
        public void CreateDirectory(string path);

        public void SaveFile(string path, byte[] fileBytes);

        public string GetHash(byte[] fileBytes);
    }
    
}
