using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerUpload7.BLL.DTO
{
    public class MaterialDTO
    {
   //     public int Id { get; set; }
        
        public string Name { get; set; }
        public string Category { get; set; }

        public List<VersionDTO> Versions { get; set; }

        public  MaterialDTO()
        {
            this.Versions = new List<VersionDTO>();
        }

    }
}
