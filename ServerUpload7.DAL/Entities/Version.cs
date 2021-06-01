using System;

namespace ServerUpload.DAL.Entities
{
    public class Version
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime UploadTime { get; set; }

        public string StrHash { get; set; }
        public long FileSize { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }

    }
}
