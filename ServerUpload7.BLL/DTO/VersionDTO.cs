using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerUpload7.BLL.DTO
{
    public class VersionDTO
    {
 //       public int Id { get; set; }

        public string Name { get; set; }
        public DateTime UploadTime { get; set; }

        public long FileSize { get; set; }
        public int MaterialId { get; set; }
        public MaterialDTO Material { get; set; }

    }
}
