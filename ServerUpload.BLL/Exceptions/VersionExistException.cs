using System;

namespace ServerUpload.BLL.Exceptions
{
    public class VersionExistException : Exception
    {
        public VersionExistException(string message) : base(message)
        {
        }
    }
}