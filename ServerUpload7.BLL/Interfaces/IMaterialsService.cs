using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ServerUpload7.BLL.ModelDTO;
using AutoMapper;

namespace ServerUpload7.BLL.Interfaces
{
    public interface IMaterialsService : ICommon
    {
        public MaterialDTO CreateMaterial(Byte[] FileBytes, string Category, string FileName, long Size, IMapper _mapper, string path, string StrHash);
        public string GetPath(string category, string FileName, int number, IMapper _mapper, string StrHash);
        public string DownloadActualVersion(string Name, string Category, IMapper _mapper);
        public MaterialDTO GetMaterialInfo(string Name, string Category, IMapper _mapper);
        public int ChangeCategory(string Name, string OldCategory, string NewCategory, IMapper _mapper);
        public IEnumerable<MaterialDTO> FilterMat(string Category, IMapper _mapper);
    }
}
