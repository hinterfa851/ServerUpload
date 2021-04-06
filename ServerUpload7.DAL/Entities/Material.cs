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
        public string Category { get; set; }
        public List<Version> Versions { get; set; }

   /*     public Material()
        {
            this.Versions = new List<Version>();
        }*/
    }
}
