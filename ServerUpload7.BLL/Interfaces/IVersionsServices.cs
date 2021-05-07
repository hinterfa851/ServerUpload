using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServerUpload7.BLL.ModelDTO;

namespace ServerUpload7.BLL.Interfaces
{   
    public interface IVersionsService : ICommon
    {
        public VersionDTO CreateVersion(byte [] FileBytes, string Name, string Category, long Size, IMapper _mapper, string path, string StrHash, string FileName);
        public string DownloadVers(int number, string name, string category, IMapper _mapper);

        public string GetPath(string category, string MatName, IMapper _mapper, string StrHash, string FileName);
    }
}
