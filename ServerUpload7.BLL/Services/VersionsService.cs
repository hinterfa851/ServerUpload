using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.Services;
using ServerUpload7.DAL.Entities;
using Version = ServerUpload7.DAL.Entities.Version;
using System.IO;
using ServerUpload7.DAL.Interfaces;
using AutoMapper;

namespace ServerUpload7.BLL.Services
{
    public class VersionsService : IVersionsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VersionsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        private string CrName(string Input, int Num_v)
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
        public string GetPath(string category, string FileName)
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

                if (Mat != null)            // ********** CHEK THIS ONE *********
                {
                    if (helper == null)
                        return "Files/" + category + "/" + DirName + "/" + FileName + $"_v{Mat.Versions.Count + 1}";
                    else
                        return "Files/" + category + "/" + DirName + "/" + CrName(FileName, Mat.Versions.Count + 1);
                }
            }
            return null;
        }
        public Version CreateVersion(string Name, string Category, string WebRootPath, long Size)
        {
            if ((Category == "App" || Category == "Presentation" || Category == "Other"))      // !(_context.Files.Any(Mat => Mat.Name == uploadedFile.FileName) 
            {
                var Mat = _unitOfWork.Materials.Find(u => u.Category == Category && u.Name == Name);
                if (Mat == null)
                    return null;
                var Vers = new Version { Name = CrName(Name, Mat.Versions.Count + 1), FileSize = Size, UploadTime = DateTime.Now, Material = Mat };
                Mat.Versions.Add(Vers);
                _unitOfWork.Versions.Create(Vers);
                _unitOfWork.Versions.Save();
                return (Vers);
            }
            return (null);
        }
        public string DownloadVers(int number, string Name, string Category)
        {
            if (Category == "App" || Category == "Presentation" || Category == "Other")
            {
                var material = _unitOfWork.Materials.Find(m => m.Name == Name && m.Category == Category); // may not work properly
                if (material == null)
                    return null;
                var version = material.Versions[number - 1];
                return Category + "/" + CrNameWithoutExt(Name) + "/" + version.Name; // + "/" + version.extention
            }
            return null;
        }

    }
}
