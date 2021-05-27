using System;
using System.Linq;
using ServerUpload7.BLL.Interfaces;
using ServerUpload7.DAL.Interfaces;
using AutoMapper;
using ServerUpload7.BLL.Enums;
using ServerUpload7.BLL.Exceptions;
using DataVersion = ServerUpload7.DAL.Entities.Version;
using Version = ServerUpload7.BLL.BusinessModels.Version;


namespace ServerUpload7.BLL.Services
{
    public class VersionsService : ServiceBase, IVersionsService
    {
        private readonly IMapper _mapper;

        public VersionsService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            this._mapper = mapper;
        }
        public string GetPath(byte category, string materialName, string hashString, string fileName)
        {
            if (!Enum.IsDefined(typeof(Categories), category))
                throw new CategoryException("Invalid category. Use: App - 0, Presentation - 1, Other - 2.");
            var material = _unitOfWork.Materials.Find(m => m.Name == materialName && m.Category == category);
            if (material == null)
                throw new MaterialNotExistException("Adding new version to unexisted material, try to create material first");
            string directoryName = GetName(materialName);
            foreach (var v in material.Versions)
            {
                var version = _mapper.Map<Version>(v);
                if (version.HashString == hashString)
                    throw new VersionExistException("This version of material is already created, try to change it and upload again");
            }
            if (!fileName.Contains('.'))
                return "Files/"  + directoryName + "/" + fileName + $"_v{material.Versions.Count + 1}";
            else
                return "Files/"  + directoryName + "/" + GetVersion(fileName, materialName, material.Versions.Count + 1);

        }
        public Version CreateVersion(byte [] fileBytes, string name, byte category, long size, string path, string strHash, string fileName)
        {
            var material = _unitOfWork.Materials.Find(u => u.Category == category && u.Name == name);
            /*
            var Mat = _mapper.Map<MaterialDTO>(Material);
            var Vers = new VersionDTO { Name = ICommon.GetVersion(FileName, Name, Mat.Versions.Count), hashString = hashString, FileSize = Size, UploadTime = DateTime.Now, Material = Mat };
            //         _unitOfWork.Versions.Create(_mapper.Map<Version>(Vers), FileBytes, path);

            //           _unitOfWork.Materials.Update(_mapper.Map<Material>(Mat)); // порядок??

            Mat.Versions.Add(Vers);
            */
            var version = new DataVersion { Name = GetVersion(fileName, name, material.Versions.Count + 1), StrHash = strHash, FileSize = size, UploadTime = DateTime.Now, Material = material };
            material.Versions.Add(version);
            _unitOfWork.FileManager.SaveFile(path, fileBytes);
            _unitOfWork.Versions.Create(version);
            _unitOfWork.Materials.Update(material);
            _unitOfWork.Versions.Save();
            return (_mapper.Map<Version>(version));
                
        }
        public string DownloadVersion(int number, string name, byte category)
        {
            if (!Enum.IsDefined(typeof(Categories), category))
                throw new CategoryException("Invalid category. Use: App - 0, Presentation - 1, Other - 2.");
            var material = _unitOfWork.Materials.Find(m => m.Name == name && m.Category == category); 
            if (material == null)
                throw new MaterialNotExistException("Downloading version of unexisted material, try to create material first");
            var version = material.Versions.ElementAt(number - 1);          
            return GetName(name) + "/" + version.Name;
        }
    }
}
