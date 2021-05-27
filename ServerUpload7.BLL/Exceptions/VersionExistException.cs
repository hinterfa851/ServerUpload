using System;

namespace ServerUpload7.BLL.Exceptions
{
    public class VersionExistException : Exception
    {
        public VersionExistException(string message) : base(message)
        {
        }
    }
}