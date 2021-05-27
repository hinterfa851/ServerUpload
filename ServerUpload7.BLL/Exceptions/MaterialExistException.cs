using System;

namespace ServerUpload7.BLL.Exceptions
{
    public class MaterialExistException : Exception
    {
        public MaterialExistException(string message) : base(message)
        {
        }
    }
}