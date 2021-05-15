using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerUpload7.DAL.Repositories
{
    public class FileManager
    {
        public void SaveFile(string path, byte [] fileBytes)
        {
            File.WriteAllBytes(path, fileBytes);
        }
    }
}
