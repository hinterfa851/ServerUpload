using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.Interfaces;
using ServerUpload7.BLL.ModelDTO;
using System.IO;
using Version = ServerUpload7.DAL.Entities.Version;
using ServerUpload7.BLL.Interfaces;
using AutoMapper;
using ServerUpload7.DAL.Entities;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ServerUpload7.BLL.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public string GetPath(string category, string FileName, int number, IMapper _mapper, string StrHash)
        {
            if (_unitOfWork.GetCategories().Any(m => m == category))
            {                 
                foreach (var v in _unitOfWork.Versions.GetAll(x => x.StrHash != null))
                {
                    var ver = _mapper.Map<VersionDTO>(v);
                    if (ver.StrHash == StrHash)
                        return null;
                }
                var Mat = _mapper.Map<MaterialDTO>(_unitOfWork.Materials.Find(u => u.Category == category && u.Name == FileName));
                if (Mat == null)
                {
                    Directory.CreateDirectory("C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + category + "/" + ICommon.GetName(FileName));
                    return "Files/" + category + "/" + ICommon.GetName(FileName) + "/" + ICommon.GetVersion(FileName, FileName, number);
                }
            }
            return null;
        }

        public MaterialDTO CreateMaterial(Byte [] FileBytes, string Category, string FileName, long Size, IMapper _mapper, string path, string StrHash)
        {
            if (_unitOfWork.GetCategories().Any(m => m == Category))
            {
                Directory.CreateDirectory("C:/Users/My/source/repos/ServerUpload7/Files/" + Category + "/" + FileName);

                var Mat = new MaterialDTO { Name = FileName, Category = Category };
                var Vers = new VersionDTO { Name = ICommon.GetVersion(FileName, FileName, 1), StrHash = StrHash, FileSize = Size, UploadTime = DateTime.Now, Material = Mat };
               
                Mat.Versions = new List<VersionDTO>();   
                Mat.Versions.Add(Vers);
                _unitOfWork.Materials.Create(_mapper.Map<Material>(Mat), FileBytes, path);
                return (Mat);
            }
            return (null);
        }

        public string DownloadActualVersion(string Name, string Category, IMapper _mapper)
        {
            if (_unitOfWork.GetCategories().Any(m => m == Category))
            {
                var material =  _unitOfWork.Materials.Find(m =>  m.Name == Name && m.Category == Category); 
                if (material == null)
                    return null;
               var version = material.Versions.Last();
               return Category + "/" + ICommon.GetName(Name) + "/" + version.Name;
            }
            return null;
        }

        public MaterialDTO GetMaterialInfo(string Name, string Category, IMapper _mapper)
        {
            if (_unitOfWork.GetCategories().Any(m => m == Category))
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == Name && m.Category == Category); 
                return _mapper.Map<MaterialDTO>(material);
            }
            return null;
        }

        public int ChangeCategory(string Name, string OldCategory, string NewCategory, IMapper _mapper)
        {
            if ((_unitOfWork.GetCategories().Any(m => m == OldCategory)) && 
                (_unitOfWork.GetCategories().Any(m => m == NewCategory)) && OldCategory != NewCategory)
            {
                // check material
                var material = _mapper.Map<MaterialDTO>(_unitOfWork.Materials.Find(m => m.Name == Name && m.Category == OldCategory)); // may not work properly
                var material2 = _mapper.Map<MaterialDTO>(_unitOfWork.Materials.Find(m => m.Name == Name && m.Category == NewCategory));
                if (material2 != null || material == null)
                {
                    return 0;
                }
                material.Category = NewCategory;
                _unitOfWork.Materials.Update(_mapper.Map<Material>(material));
                var DirName = ICommon.GetName(material.Name);
                Directory.Move("C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + OldCategory + "/" + DirName, "C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + NewCategory + "/" + DirName);
                return 1;
            }
            return 0;
        }

        public IEnumerable<MaterialDTO> FilterMat(string Category, IMapper _mapper)
        {
            if (_unitOfWork.GetCategories().Any(m => m == Category))
            {

                var Materials = _unitOfWork.Materials.GetAll(m => m.Category == Category);
                List<MaterialDTO> result = new List<MaterialDTO>();
                foreach (var i in Materials)
                {
                    result.Add(_mapper.Map<MaterialDTO>(i));
                }
                return result.AsEnumerable();
            }
            return null;

        }

    }
}
