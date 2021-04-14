using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.Interfaces;
using ServerUpload7.DAL.Entities;
using System.IO;
using Version = ServerUpload7.DAL.Entities.Version;
using ServerUpload7.DAL.Services;

namespace ServerUpload7.BLL.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public string CrName(string Input, int Num_v)
        {
            string Result;
            var rr = Input.Split(".");
            int len = rr.Length;
            string NewStr = "";
            int i = 0;
            while (i < len - 1)
            {
                NewStr += rr[i];
                i++;
            }
            if (Num_v == 1)
            {
                Result = $"{NewStr}_v{Num_v}.{rr[i]}";
            }
            else
                Result = $"{NewStr}_v{Num_v}.{rr.Last()}";
            return Result;
        }
        private string CrNameWithoutExt(string FileName)
        {
            string NewStr = "";
            int i = 0;
            var arr = FileName.Split(".");
            int len = arr.Length;

            while (i < len - 1)
            {
                NewStr += arr[i];
                i++;
            }
            return NewStr;
        }

        public string GetPath(string category, string FileName, int number)
        {
            if ((category == "App" || category == "Presentation" || category == "Other"))
            {
                var helper = FileName.Split(".");
                string DirName;

                if (helper == null)
                    DirName = FileName;
                else
                    DirName = CrNameWithoutExt(FileName);

                var Mat = _unitOfWork.Materials.Find(u => u.Category == category && u.Name == FileName);
                if (Mat == null)
                {
                    Directory.CreateDirectory("C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + category + "/" + DirName);
                    return "Files/" + category + "/" + DirName + "/" + CrName(FileName, number);
                }
            }
            return null;
        }

        public Material CreateMaterial(Stream uploadedFile, string Category, string FileName, string WebRootPath, long Size)
        {
            Console.WriteLine($"v1{Category}");
            if ((Category == "App" || Category == "Presentation" || Category == "Other"))      // !(_context.Files.Any(Mat => Mat.Name == uploadedFile.FileName) 
            {
                Directory.CreateDirectory("C:/Users/My/source/repos/ServerUpload7/Files/" + Category + "/" + FileName);
                var Mat = new Material { Name = FileName, Category = Category };
                var Vers = new Version { Name = CrName(FileName, 1), FileSize = Size, UploadTime = DateTime.Now, Material = Mat };
                Mat.Versions = new List<Version>();
                Mat.Versions.Add(Vers);

                _unitOfWork.Materials.Create(Mat);
                return (Mat);
            }
            Console.WriteLine($"First contition falls {Category} ");
            return (null);
        }

        public string DownloadActualVersion(string Name, string Category)
        {
            if (Category == "App" || Category == "Presentation" || Category == "Other")
            {
               var material =  _unitOfWork.Materials.Find(m =>  m.Name == Name && m.Category == Category); // may not work properly
                if (material == null)
                    return null;
               var version = material.Versions.Last();
               return Category + "/" + CrNameWithoutExt(Name) + "/" + version.Name; 
            }
            return null;
        }

        public Material GetMaterialInfo(string Name, string Category)
        {
            if (Category == "App" || Category == "Presentation" || Category == "Other")
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == Name && m.Category == Category); // may not work properly
                return material;
            }
            return null;
        }

        public int ChangeCategory(string Name, string OldCategory, string NewCategory)
        {
            if ((OldCategory == "App" || OldCategory == "Presentation" || OldCategory == "Other") && (NewCategory == "App" || NewCategory == "Presentation" || NewCategory == "Other") && OldCategory != NewCategory)
            {
                // check material
                var material = _unitOfWork.Materials.Find(m => m.Name == Name && m.Category == OldCategory); // may not work properly
                var material2 = _unitOfWork.Materials.Find(m => m.Name == Name && m.Category == NewCategory);
                if (material2 != null || material == null)
                {
                    return 0;
                }
                material.Category = NewCategory;
                _unitOfWork.Materials.Save();
                Directory.Move("C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + OldCategory + "/" + CrNameWithoutExt(material.Name), "C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/" + NewCategory + "/" + CrNameWithoutExt(material.Name));
                return 1;
            }
            return 0;
        }

        public IEnumerable<Material> FilterMat(string Category)
        {
            if ((Category == "App" || Category == "Presentation" || Category == "Other"))
            {

                var Materials = _unitOfWork.Materials.GetAll(m => m.Category == Category);
                return Materials;
            }
            return null;

        }

    }
}
