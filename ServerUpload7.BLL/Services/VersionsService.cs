using System;
using System.Linq;
using ServerUpload7.BLL.Interfaces;
using ServerUpload7.DAL.Interfaces;
using AutoMapper;
using DataVersion = ServerUpload7.DAL.Entities.Version;
using Version = ServerUpload7.BLL.BusinessModels.Version;
using Category = ServerUpload7.BLL.BusinessModels.Category;


namespace ServerUpload7.BLL.Services
{
    public class VersionsService : ServiceBase, IVersionsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VersionsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /*
        public string GetHash(byte[] fileBytes)
        {
            string result;

            result = _unitOfWork.FileManager.GetHash(fileBytes);
            return result;
        }
        */
        public string GetPath(int category, string materialName, IMapper mapper, string hashString, string fileName)
        {
            Category dbCategory = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category));

            if (dbCategory != null)
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == materialName && m.Category == dbCategory.Name);
                if (material != null)            
                {
                    string directoryName = GetName(materialName);
                    foreach (var v in material.Versions)
                    {
                        var version = mapper.Map<Version>(v);
                        if (version.HashString == hashString)
                            return null;
                    }
                    if (!fileName.Contains('.'))
                        return "Files/" + category + "/" + directoryName + "/" + fileName + $"_v{material.Versions.Count + 1}";
                    else
                        return "Files/" + category + "/" + directoryName + "/" + GetVersion(fileName, materialName, material.Versions.Count + 1);
                }
            }
            return null;
        }
        public Version CreateVersion(byte [] fileBytes, string name, int category, long size, IMapper mapper, string path, string strHash, string fileName)
        {
            Category dbCategory = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category));

            if (dbCategory != null)
            {
                var Material = _unitOfWork.Materials.Find(u => u.Category == dbCategory.Name && u.Name == name);
                if (Material == null)
                    return null;
                /*
                var Mat = _mapper.Map<MaterialDTO>(Material);
                var Vers = new VersionDTO { Name = ICommon.GetVersion(FileName, Name, Mat.Versions.Count), hashString = hashString, FileSize = Size, UploadTime = DateTime.Now, Material = Mat };
                //         _unitOfWork.Versions.Create(_mapper.Map<Version>(Vers), FileBytes, path);

                //           _unitOfWork.Materials.Update(_mapper.Map<Material>(Mat)); // порядок??

                Mat.Versions.Add(Vers);
                */

                var version = new DataVersion { Name = GetVersion(fileName, name, Material.Versions.Count), StrHash = strHash, FileSize = size, UploadTime = DateTime.Now, Material = Material };
                Material.Versions.Add(version);

                _unitOfWork.FileManager.SaveFile(path, fileBytes);
                _unitOfWork.Versions.Create(version);
                _unitOfWork.Materials.Update(Material);
                _unitOfWork.Versions.Save();
                return (mapper.Map<Version>(version));
            }
            return (null);
        }
        public string DownloadVersion(int number, string name, int category, IMapper mapper)
        {
            Category dbCategory = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category));

            if (dbCategory != null)
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == name && m.Category == dbCategory.Name); 
                if (material == null)
                    return null;
                var version = material.Versions.ElementAt(number - 1);          
                return category + "/" + GetName(name) + "/" + version.Name;
            }
            return null;
        }

    }
}
