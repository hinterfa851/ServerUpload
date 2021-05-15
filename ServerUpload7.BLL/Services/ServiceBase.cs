using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ServerUpload7.DAL.Interfaces;

namespace ServerUpload7.BLL.Services
{
    public abstract class ServiceBase
    {
        private static readonly Regex Regex1 = new Regex(@"^.*(?=\.)");
        private static readonly Regex Regex2 = new Regex(@"[^.]*$");
        private readonly IUnitOfWork _unitOfWork;

        
        public static string GetVersion(string fileName, string matName, int number)
        {
            string DirName;

            if (!fileName.Contains('.'))
                DirName = matName + $"_v{number}";
            else
                DirName = Regex1.Match(matName).Value + $"_v{number}." + Regex2.Match(fileName).Value;
            return DirName;
        }
        public static string GetName(string fileName)
        {
            return !fileName.Contains('.') ? fileName : Regex1.Match(fileName).Value;
        }

        public string GetHash(byte[] fileBytes)
        {
            var result = _unitOfWork.FileManager.GetHash(fileBytes);
            return result;
        }

    }
}
