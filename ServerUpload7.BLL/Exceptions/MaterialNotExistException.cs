using System;

namespace ServerUpload.BLL.Exceptions
{
    public class MaterialNotExistException : Exception
    {
        public MaterialNotExistException(string message) : base(message)
        {
        }
    }
}