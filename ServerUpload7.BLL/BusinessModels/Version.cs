using System;

namespace ServerUpload7.BLL.BusinessModels
{
    public class Version
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HashString { get; set; }
        public DateTime UploadTime { get; set; }
        public long FileSize { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
    }
}