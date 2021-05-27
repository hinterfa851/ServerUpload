using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerUpload7.DAL.Entities
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public ICollection<Version> Versions { get; set; }

    }
}
