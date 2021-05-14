using System.Text.RegularExpressions;

namespace ServerUpload7.BLL.Interfaces
{
    public interface ICommon
    {
        private static readonly Regex Regex1 = new Regex(@"^.*(?=\.)");
        private static readonly Regex Regex2 = new Regex(@"[^.]*$");

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


    }
}
