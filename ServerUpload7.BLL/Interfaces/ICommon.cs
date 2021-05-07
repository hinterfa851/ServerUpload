using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ServerUpload7.BLL.Interfaces
{
    public interface ICommon
    {
        private static Regex regex1 = new Regex(@"^.*(?=\.)");
        private static Regex regex2 = new Regex(@"[^.]*$");

        public static string GetVersion(string FileName, string MatName, int number)
        {
            string DirName;

            if (!FileName.Contains('.'))
                DirName = MatName + $"_v{number}";
            else
                DirName = regex1.Match(MatName).Value + $"_v{number}." + regex2.Match(FileName).Value;
            return DirName;
        }
        public static string GetName(string FileName)
        {
            string DirName;

            if (!FileName.Contains('.'))
                DirName = FileName;
            else
                DirName = regex1.Match(FileName).Value;
            return DirName;
        }


    }
}
