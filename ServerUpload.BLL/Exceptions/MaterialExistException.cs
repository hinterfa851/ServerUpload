using System;

namespace ServerUpload.BLL.Exceptions
{
    public class MaterialExistException : Exception
    {
        public MaterialExistException(string message) : base(message)
        {
        }
    }
}