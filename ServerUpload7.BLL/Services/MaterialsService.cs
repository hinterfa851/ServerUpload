using System;
using System.Collections.Generic;
using System.Linq;
using ServerUpload7.DAL.Interfaces;
using System.IO;
using ServerUpload7.BLL.Interfaces;
using AutoMapper;
using DataMaterial = ServerUpload7.DAL.Entities.Material;
using Material = ServerUpload7.BLL.BusinessModels.Material;


namespace ServerUpload7.BLL.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public string GetPath(int category, string fileName, int number, IMapper mapper, string hashString)
        {
            if (_unitOfWork.GetCategories().Any(m => m == category))
            {                 
                foreach (var v in _unitOfWork.Versions.GetAll(x => x.StrHash != null))
                {
                    var ver = mapper.Map<BusinessModels.Version>(v);
                    if (ver.HashString == hashString)
                        return null;
                }
                var Mat = mapper.Map<Material>(_unitOfWork.Materials.Find(u => u.Category == category && u.Name == fileName));
                if (Mat == null)
                {
                    Directory.CreateDirectory("C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + category + "/" + ICommon.GetName(fileName));
                    return "Files/" + category + "/" + ICommon.GetName(fileName) + "/" + ICommon.GetVersion(fileName, fileName, number);
                }
            }
            return null;
        }

        public Material CreateMaterial(byte [] fileBytes, int category, string fileName, long size, IMapper mapper, string path, string hashString)
        {
            if (_unitOfWork.GetCategories().Any(m => m == category))
            {
                Directory.CreateDirectory("C:/Users/My/source/repos/ServerUpload7/Files/" + category + "/" + fileName);

                var Material = new Material { Name = fileName, Category = category };
                var Version = new BusinessModels.Version { Name = ICommon.GetVersion(fileName, fileName, 1), HashString = hashString, FileSize = size, UploadTime = DateTime.Now, Material = Material };

                Material.Versions = new List<BusinessModels.Version> {Version};
                _unitOfWork.Materials.Create(mapper.Map<DataMaterial>(Material), fileBytes, path);
                return (Material);
            }
            return (null);
        }

        public string DownloadActualVersion(string name, int category, IMapper mapper)
        {
            if (_unitOfWork.GetCategories().Any(m => m == category))
            {
                var material =  _unitOfWork.Materials.Find(m =>  m.Name == name && m.Category == category); 
                if (material == null)
                    return null;
                var version = material.Versions.Last();
                return category + "/" + ICommon.GetName(name) + "/" + version.Name;
            }
            return null;
        }

        public Material GetMaterialInfo(string name, int category, IMapper mapper)
        {
            if (_unitOfWork.GetCategories().Any(m => m == category))
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == name && m.Category == category); 
                return mapper.Map<Material>(material);
            }
            return null;
        }

        public int ChangeCategory(string name, int oldCategory, int newCategory, IMapper mapper)
        {
            if ((_unitOfWork.GetCategories().Any(m => m == oldCategory)) && 
                (_unitOfWork.GetCategories().Any(m => m == newCategory)) && oldCategory != newCategory)
            {
                // check material
                var material = mapper.Map<Material>(_unitOfWork.Materials.Find(m => m.Name == name && m.Category == oldCategory)); 
                var material2 = mapper.Map<Material>(_unitOfWork.Materials.Find(m => m.Name == name && m.Category == newCategory));
                if (material2 != null || material == null)
                {
                    return 0;
                }
                material.Category = newCategory;
                _unitOfWork.Materials.Update(mapper.Map<DAL.Entities.Material>(material));
                var DirName = ICommon.GetName(material.Name);
                Directory.Move("C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + oldCategory + "/" + DirName, "C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + newCategory + "/" + DirName);
                return 1;
            }
            return 0;
        }

        public IEnumerable<Material> FilterMat(int category, IMapper mapper)
        {
            if (_unitOfWork.GetCategories().Any(m => m == category))
            {

                var Materials = _unitOfWork.Materials.GetAll(m => m.Category == category);
                var result = Materials.Select(mapper.Map<Material>).ToList();
                return result.AsEnumerable();
            }
            return null;
        }

    }
}
