using System;

namespace ServerUpload7.BLL.Exceptions
{
    public class MaterialNotExistException : Exception
    {
        public MaterialNotExistException(string message) : base(message)
        {
        }
    }
}