using System;
using System.Collections.Generic;
using System.Linq;
using ServerUpload7.DAL.Interfaces;
using System.IO;
using ServerUpload7.BLL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using DataVersion = ServerUpload7.DAL.Entities.Version;
using DataMaterial = ServerUpload7.DAL.Entities.Material;
using Material = ServerUpload7.BLL.BusinessModels.Material;
using Category = ServerUpload7.BLL.BusinessModels.Category;

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

        public string GetPath(int category, string fileName, int number, string hashString)
        { 
            Category dbCategory = _mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category + 1));
            if (dbCategory != null)
            {
                var tet = _unitOfWork.Versions.GetAll(x => x.StrHash != null);
                foreach (var v in tet)
                {
                    var ver = _mapper.Map<BusinessModels.Version>(v);
                    if (ver.HashString == hashString)
                        return null;
                }
                var Mat = _mapper.Map<Material>(_unitOfWork.Materials.Find(u => u.Category == dbCategory.Name && u.Name == fileName));
                if (Mat == null)
                {
                    var path = _configuration.GetSection("FilePath").Value;
                     Directory.CreateDirectory( path + dbCategory.Name + "/" + GetName(fileName));
                    return "Files/" + dbCategory.Name + "/" + GetName(fileName) + "/" + GetVersion(fileName, fileName, number);
                }
            }
            return null;
        }

        public Material CreateMaterial(byte [] fileBytes, int category, string fileName, long size, string path, string hashString)
        {
            Category dbCategory = _mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category + 1));
            if (dbCategory != null)
            {
                var Material = new Material { Name = fileName, Category = dbCategory.Name };
                var Version = new BusinessModels.Version { Name = GetVersion(fileName, fileName, 1), HashString = hashString, FileSize = size, UploadTime = DateTime.Now, Material = Material };

                Material.Versions = new List<BusinessModels.Version> {Version};
                try
                {
                    _unitOfWork.FileManager.SaveFile(path, fileBytes);
                }
                catch (Exception e)
                {
                    return null;
                }
                _unitOfWork.Materials.Create(_mapper.Map<DataMaterial>(Material));
                return (Material);
            }
            return (null);
        }

        public string DownloadActualVersion(string name, int category)
        {
            Category dbCategory = _mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category + 1));
            if (dbCategory != null)
            {
                var material =  _unitOfWork.Materials.Find(m =>  m.Name == name && m.Category == dbCategory.Name); 
                if (material == null)
                    return null;
                var version = material.Versions.Last();
                return dbCategory.Name + "/" + GetName(name) + "/" + version.Name;
            }
            return null;
        }

        public Material GetMaterialInfo(string name, int category)
        {
            Category dbCategory = _mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category + 1));

            if (dbCategory != null)
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == name && m.Category == dbCategory.Name); 
                return _mapper.Map<Material>(material);
            }
            return null;
        }

        public int ChangeCategory(string name, int oldCategory, int newCategory)
        {
            Category category1 = _mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == oldCategory + 1));
            Category category2 = _mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == newCategory + 1));

            if (category1 != null && category2 != null && oldCategory != newCategory)
            {
                // check material
                var path = _configuration.GetSection("FilePath").Value;
                var material = _mapper.Map<Material>(_unitOfWork.Materials.Find(m => m.Name == name && m.Category == category1.Name)); 
                var material2 = _mapper.Map<Material>(_unitOfWork.Materials.Find(m => m.Name == name && m.Category == category2.Name));
                if (material2 != null || material == null)
                {
                    return 0;
                }
                material.Category = category2.Name;
                _unitOfWork.Materials.Update(_mapper.Map<DAL.Entities.Material>(material));
                var DirName = GetName(material.Name);
                try
                {
                    Directory.Move(path + category1.Name + "/" + DirName, path + category2.Name + "/" + DirName);
                }
                catch (Exception e)
                {
                    return 0;
                }
                return 1;
            }
            return 0;
        }

        public IEnumerable<Material> FilterMat(int category)
        {
            Category dbCategory = _mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category + 1));

            if (dbCategory != null)
            {

                var Materials = _unitOfWork.Materials.GetAll(m => m.Category == dbCategory.Name);
                var result = Materials.Select(_mapper.Map<Material>).ToList();
                return result.AsEnumerable();
            }
            return null;
        }
    }
}
