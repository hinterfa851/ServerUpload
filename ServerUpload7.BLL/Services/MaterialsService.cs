using System;
using System.Collections.Generic;
using System.Linq;
using ServerUpload7.DAL.Interfaces;
using System.IO;
using ServerUpload7.BLL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ServerUpload7.BLL.Enums;
using ServerUpload7.BLL.Exceptions;
using DataVersion = ServerUpload7.DAL.Entities.Version;
using DataMaterial = ServerUpload7.DAL.Entities.Material;
using Material = ServerUpload7.BLL.BusinessModels.Material;

namespace ServerUpload7.BLL.Services
{
    public class MaterialsService : ServiceBase, IMaterialsService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
  

        public MaterialsService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper) : base(unitOfWork)
        {
            this._configuration = configuration;
            this._mapper = mapper;
        }

        public string GetPath(Categories category, string fileName, int number, string hashString)
        {
            if (!Enum.IsDefined(typeof(Categories), category))
                throw new CategoryException("Invalid category. Use: App - 0, Presentation - 1, Other - 2.");
            foreach (var version in _unitOfWork.Versions.GetAll(x => x.StrHash != null))
            {
                var mappedVersion = _mapper.Map<BusinessModels.Version>(version);
                if (mappedVersion.HashString == hashString)
                    throw new VersionExistException("This version of material is already created, try to change it and upload again");
            }
            var material = _mapper.Map<Material>(_unitOfWork.Materials.Find(u =>
                u.Category == (int) category && u.Name == fileName));
            if (material != null)
                throw new MaterialExistException("Material is created already, try to create another one");
            var path = _configuration.GetSection("FilePath").Value;
            _unitOfWork.FileManager.CreateDirectory(path + GetName(fileName));
            return "Files/" + GetName(fileName) + "/" + GetVersion(fileName, fileName, number);
        }

        public Material CreateMaterial(byte [] fileBytes, Categories category, string fileName, long size, string path, string hashString)
        {
            var material = new Material { Name = fileName, Category = (int) category };
            var version = new BusinessModels.Version { Name = GetVersion(fileName, fileName, 1), HashString = hashString, FileSize = size, UploadTime = DateTime.Now, Material = material };
            material.Versions = new List<BusinessModels.Version> {version};
            _unitOfWork.FileManager.SaveFile(path, fileBytes);
            _unitOfWork.Materials.Create(_mapper.Map<DataMaterial>(material));
            return (material);
        }

        public string DownloadActualVersion(string name, Categories category)
        {
            if (!Enum.IsDefined(typeof(Categories), category))
                throw new CategoryException("Invalid category. Use: App - 0, Presentation - 1, Other - 2.");
            var material =  _unitOfWork.Materials.Find(m =>  m.Name == name && m.Category == (int) category); 
            if (material == null)
                return null;
            var version = material.Versions.Last();
            return GetName(name) + "/" + version.Name;
        }

        public Material GetMaterialInfo(string name, Categories category)
        {
            if (!Enum.IsDefined(typeof(Categories), category))
                throw new CategoryException("Invalid category. Use: App - 0, Presentation - 1, Other - 2.");
            var material = _unitOfWork.Materials.Find(m => m.Name == name && m.Category == (int) category); 
            return _mapper.Map<Material>(material);
        }

        public Material ChangeCategory(string name, Categories oldCategory, Categories newCategory)
        {
            if (!Enum.IsDefined(typeof(Categories), oldCategory) || !Enum.IsDefined(typeof(Categories), newCategory) ||
                oldCategory == newCategory)
                throw new CategoryException("Invalid category. Use: App - 0, Presentation - 1, Other - 2.");
            // check material
            var material = _mapper.Map<Material>(_unitOfWork.Materials.Find(m => m.Name == name && m.Category == (int) oldCategory)); 
            var material2 = _mapper.Map<Material>(_unitOfWork.Materials.Find(m => m.Name == name && m.Category == (int) newCategory));
            if ( material == null)
                throw new MaterialNotExistException("Changing category of unexisted material, try to create material first");
            if (material2 != null)
                throw new MaterialExistException("This material name is already exist in new category");
            material.Category = (int) newCategory;
            var updatedMaterial = _mapper.Map<Material>(_unitOfWork.Materials.Update(_mapper.Map<DAL.Entities.Material>(material)));
            return updatedMaterial;
        }

        public IEnumerable<Material> FilterMat(Categories category)
        {
            if (!Enum.IsDefined(typeof(Categories), category))
                throw new CategoryException("Invalid category. Use: App - 0, Presentation - 1, Other - 2.");
            var materials = _unitOfWork.Materials.GetAll(m => m.Category == (int) category);
            var result = materials.Select(_mapper.Map<Material>).ToList();
            return result.AsEnumerable();
        }
    }
}
