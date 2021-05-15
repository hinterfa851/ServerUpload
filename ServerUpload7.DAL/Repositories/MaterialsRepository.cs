using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.EF;
using ServerUpload7.DAL.Entities;
using ServerUpload7.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ServerUpload7.DAL.Repositories
{
    public class MaterialsRepository : IRepository<Material>
    {
        private ApplicationContext db;

        public MaterialsRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Material> GetAll(Func<Material, Boolean> predicate)
        {
            var test = db.Materials.Include(o => o.Versions);
            return test.Where(predicate);
        }

        public Material Get(int id)
        {
            return db.Materials.Find(id);
        }

        public Material Create(Material material)
        {
            db.Materials.Add(material);
            db.SaveChanges();
            return (material);
        }
     
        public Material Find(Func<Material, Boolean> predicate)
        {
            var res =  db.Materials.Include(o => o.Versions);
            return res.FirstOrDefault(predicate);
        }

        public Material Update(Material material)
        {
            var mat = db.Materials.FirstOrDefault(m => m.Id == material.Id);
            mat.Name = material.Name;
            mat.Category = material.Category;
            mat.Versions = material.Versions;
            db.SaveChanges();
            return mat;
        }
        public async void Delete(int id)
        {
            var to_del = await db.Materials.FindAsync(id);
            if (to_del != null)
                db.Materials.Remove(to_del);
        }

        public void Save()
        {
            db.SaveChangesAsync();
        }
    }
}
