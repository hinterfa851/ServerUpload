using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.BLL.Interfaces;
using ServerUpload7.DAL.Entities;
using Version = ServerUpload7.DAL.Entities.Version;
using System.IO;
using ServerUpload7.DAL.Interfaces;
using AutoMapper;
using ServerUpload7.BLL.ModelDTO;

namespace ServerUpload7.BLL.Services
{
    public class VersionsService : IVersionsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VersionsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public string GetPath(string category, string MatName, IMapper _mapper, string StrHash, string FileName)
        {
            if (_unitOfWork.GetCategories().Any(m => m == category))
            {
                var Mat = _unitOfWork.Materials.Find(m => m.Name == MatName && m.Category == category);
                if (Mat != null)            
                {
                    string DirName = ICommon.GetName(MatName);
                    foreach (var v in Mat.Versions)
                    {
                        var Vers = _mapper.Map<VersionDTO>(v);
                        if (Vers.StrHash == StrHash)
                            return null;
                    }
                    if (!FileName.Contains('.'))
                        return "Files/" + category + "/" + DirName + "/" + FileName + $"_v{Mat.Versions.Count + 1}";
                    else
                        return "Files/" + category + "/" + DirName + "/" + ICommon.GetVersion(FileName, MatName, Mat.Versions.Count + 1);
                }
            }
            return null;
        }
        public VersionDTO CreateVersion(byte [] FileBytes, string Name, string Category, long Size, IMapper _mapper, string path, string StrHash, string FileName)
        {
            if (_unitOfWork.GetCategories().Any(m => m == Category))
            {
                var Material = _unitOfWork.Materials.Find(u => u.Category == Category && u.Name == Name);
                if (Material == null)
                    return null;
                /*
                var Mat = _mapper.Map<MaterialDTO>(Material);
                var Vers = new VersionDTO { Name = ICommon.GetVersion(FileName, Name, Mat.Versions.Count), StrHash = StrHash, FileSize = Size, UploadTime = DateTime.Now, Material = Mat };
                //         _unitOfWork.Versions.Create(_mapper.Map<Version>(Vers), FileBytes, path);

                //           _unitOfWork.Materials.Update(_mapper.Map<Material>(Mat)); // порядок??

                Mat.Versions.Add(Vers);
                */

                var VersTest = new Version { Name = ICommon.GetVersion(FileName, Name, Material.Versions.Count), StrHash = StrHash, FileSize = Size, UploadTime = DateTime.Now, Material = Material };
                Material.Versions.Add(VersTest);

                _unitOfWork.Versions.Create(VersTest, FileBytes, path);
                _unitOfWork.Materials.Update(Material);
                _unitOfWork.Versions.Save();
                return (_mapper.Map<VersionDTO>(VersTest));
            }
            return (null);
        }
        public string DownloadVers(int number, string Name, string Category, IMapper _mapper)
        {
            if (_unitOfWork.GetCategories().Any(m => m == Category))
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == Name && m.Category == Category); 
                if (material == null)
                    return null;
                var version = material.Versions.ElementAt(number - 1);          
                return Category + "/" + ICommon.GetName(Name) + "/" + version.Name;
            }
            return null;
        }

    }
}
