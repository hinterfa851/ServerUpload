using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerUpload7.WEB.Resources
{
    public class VersionView
    {
        public string Name { get; set; }
        public DateTime UploadTime { get; set; }

        public long FileSize { get; set; }
        public MaterialView Material { get; set; }

    }
}
