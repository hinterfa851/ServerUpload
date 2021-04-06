using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerUpload7.DAL.Entities;
using Version = ServerUpload7.DAL.Entities.Version;

namespace ServerUpload7.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<Version> Versions { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
    }
}
