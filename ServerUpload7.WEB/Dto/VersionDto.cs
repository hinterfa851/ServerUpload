using System;

namespace ServerUpload.Web.Dto
{
    public class VersionDto
    {
        public string Name { get; set; }
        public DateTime UploadTime { get; set; }

        public long FileSize { get; set; }
        public MaterialDto Material { get; set; }

    }
}
