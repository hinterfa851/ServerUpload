using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServerUpload7.DAL.Entities
{
    public class Version
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime UploadTime { get; set; }

        public long FileSize { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }

    }
}
