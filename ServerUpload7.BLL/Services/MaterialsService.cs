using System;
using System.Collections.Generic;
using System.Linq;
using ServerUpload7.DAL.Interfaces;
using System.IO;
using ServerUpload7.BLL.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ServerUpload7.DAL.Entities;
using DataMaterial = ServerUpload7.DAL.Entities.Material;
using Material = ServerUpload7.BLL.BusinessModels.Material;
using Category = ServerUpload7.BLL.BusinessModels.Category;

namespace ServerUpload7.BLL.Services
{
    public class MaterialsService : ServiceBase, IMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public MaterialsService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this._unitOfWork = unitOfWork;
            this._configuration = configuration;
        }
        public string GetPath(int category, string fileName, int number, IMapper mapper, string hashString)
        {
            Category dbCategory = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category));
            if (dbCategory != null)
            {                 
                foreach (var v in _unitOfWork.Versions.GetAll(x => x.StrHash != null))
                {
                    var ver = mapper.Map<BusinessModels.Version>(v);
                    if (ver.HashString == hashString)
                        return null;
                }
                var Mat = mapper.Map<Material>(_unitOfWork.Materials.Find(u => u.Category == dbCategory.Name && u.Name == fileName));
                if (Mat == null)
                {
                    Directory.CreateDirectory("C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + category + "/" + GetName(fileName));
                    return "Files/" + category + "/" + GetName(fileName) + "/" + GetVersion(fileName, fileName, number);
                }
            }
            return null;
        }

        public Material CreateMaterial(byte [] fileBytes, int category, string fileName, long size, IMapper mapper, string path, string hashString)
        {
            Category dbCategory = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category));
            if (dbCategory != null)
            {
                var Material = new Material { Name = fileName, Category = dbCategory.Name };
                var Version = new BusinessModels.Version { Name = GetVersion(fileName, fileName, 1), HashString = hashString, FileSize = size, UploadTime = DateTime.Now, Material = Material };

                Material.Versions = new List<BusinessModels.Version> {Version};
                _unitOfWork.FileManager.SaveFile(path, fileBytes);
                _unitOfWork.Materials.Create(mapper.Map<DataMaterial>(Material));
                return (Material);
            }
            return (null);
        }

        public string DownloadActualVersion(string name, int category, IMapper mapper)
        {
            Category dbCategory = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category));
            if (dbCategory != null)
            {
                var material =  _unitOfWork.Materials.Find(m =>  m.Name == name && m.Category == dbCategory.Name); 
                if (material == null)
                    return null;
                var version = material.Versions.Last();
                return category + "/" + GetName(name) + "/" + version.Name;
            }
            return null;
        }

        public Material GetMaterialInfo(string name, int category, IMapper mapper)
        {
            Category dbCategory = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category));

            if (dbCategory != null)
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == name && m.Category == dbCategory.Name); 
                return mapper.Map<Material>(material);
            }
            return null;
        }

        public int ChangeCategory(string name, int oldCategory, int newCategory, IMapper mapper)
        {
            Category category1 = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == oldCategory));
            Category category2 = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == newCategory));

            if (category1 != null && category2 != null && oldCategory != newCategory)
            {
                // check material
                var material = mapper.Map<Material>(_unitOfWork.Materials.Find(m => m.Name == name && m.Category == category1.Name)); 
                var material2 = mapper.Map<Material>(_unitOfWork.Materials.Find(m => m.Name == name && m.Category == category2.Name));
                if (material2 != null || material == null)
                {
                    return 0;
                }
                material.Category = category2.Name;
                _unitOfWork.Materials.Update(mapper.Map<DAL.Entities.Material>(material));
                var DirName = GetName(material.Name);
                Directory.Move("C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + oldCategory + "/" + DirName, "C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + newCategory + "/" + DirName);
                return 1;
            }
            return 0;
        }

        public IEnumerable<Material> FilterMat(int category, IMapper mapper)
        {
            Category dbCategory = mapper.Map<Category>(_unitOfWork.GetCategories().Find(m => m.Id == category));

            if (dbCategory != null)
            {

                var Materials = _unitOfWork.Materials.GetAll(m => m.Category == dbCategory.Name);
                var result = Materials.Select(mapper.Map<Material>).ToList();
                return result.AsEnumerable();
            }
            return null;
        }

    }
}
